using eurocsv.Models;
using eurocsv.Services;
using Microsoft.AspNetCore.Mvc;

namespace eurocsv.Controllers
{
    public class CsvController : Controller
    {
        private readonly ILogger<CsvController> _logger;
        private readonly ICsvTransformService _transformService;
        private readonly ITempFileService _tempFileService;

        private const long MaxFileSizeBytes = 50 * 1024 * 1024; // 50 MB

        public CsvController(
            ILogger<CsvController> logger,
            ICsvTransformService transformService,
            ITempFileService tempFileService)
        {
            _logger = logger;
            _transformService = transformService;
            _tempFileService = tempFileService;
        }

        [HttpGet]
        public IActionResult Convert()
        {
            var homeUrl = Url.Action("Index", "Home") ?? "/";
            return Redirect($"{homeUrl}#convert-workspace");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(50 * 1024 * 1024)]
        public async Task<IActionResult> Upload(IFormFile csvFile, CsvConversionOptions options)
        {
            var traceId = HttpContext.TraceIdentifier;

            var model = new ConversionSessionViewModel
            {
                Options = options,
                AvailableLocales = LocaleConvention.Presets,
                FromConvention = LocaleConvention.FindByCode(options.FromLocale),
                ToConvention = LocaleConvention.FindByCode(options.ToLocale)
            };

            // Log conversion settings at the start of each upload request.
            _logger.LogInformation(
                "ConversionRequested | TraceId={TraceId} | FromLocale={FromLocale} | ToLocale={ToLocale} | " +
                "ConvertDelimiter={ConvertDelimiter} | ConvertDecimal={ConvertDecimal} | ConvertThousands={ConvertThousands} | " +
                "HandleQualifiers={HandleQualifiers} | ConvertLineEndings={ConvertLineEndings} | OutputLineEnding={OutputLineEnding}",
                traceId, options.FromLocale, options.ToLocale,
                options.ConvertDelimiter, options.ConvertDecimalSeparator, options.ConvertThousandSeparator,
                options.HandleTextQualifiers, options.ConvertLineEndings, options.OutputLineEnding);

            if (csvFile == null || csvFile.Length == 0)
            {
                _logger.LogWarning(
                    "UploadRejected | TraceId={TraceId} | Reason=NoFileProvided",
                    traceId);
                model.ErrorMessage = "Please select a CSV file to upload.";
                return RenderHomeIndex(model);
            }

            if (csvFile.Length > MaxFileSizeBytes)
            {
                _logger.LogWarning(
                    "UploadRejected | TraceId={TraceId} | Reason=FileTooLarge | FileSize={FileSize}",
                    traceId, csvFile.Length);
                model.ErrorMessage = $"File is too large. Maximum size is {MaxFileSizeBytes / (1024 * 1024)} MB.";
                return RenderHomeIndex(model);
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning(
                    "UploadRejected | TraceId={TraceId} | Reason=InvalidModelState",
                    traceId);
                model.ErrorMessage = "Invalid conversion options.";
                return RenderHomeIndex(model);
            }

            if (model.FromConvention == null)
            {
                _logger.LogWarning(
                    "UploadRejected | TraceId={TraceId} | Reason=UnknownSourceLocale | Locale={Locale}",
                    traceId, options.FromLocale);
                model.ErrorMessage = $"Unknown source locale: {options.FromLocale}";
                return RenderHomeIndex(model);
            }

            if (model.ToConvention == null)
            {
                _logger.LogWarning(
                    "UploadRejected | TraceId={TraceId} | Reason=UnknownTargetLocale | Locale={Locale}",
                    traceId, options.ToLocale);
                model.ErrorMessage = $"Unknown target locale: {options.ToLocale}";
                return RenderHomeIndex(model);
            }

            try
            {
                // Save original — the service logs the temp file path.
                var (sessionId, originalPath) = await _tempFileService.SaveUploadAsync(csvFile);
                model.SessionId = sessionId;
                model.OriginalFileName = csvFile.FileName;
                model.OriginalFileSizeBytes = csvFile.Length;

                _logger.LogInformation(
                    "FileUploaded | TraceId={TraceId} | SessionId={SessionId} | OriginalFileName={FileName} | " +
                    "FileSizeBytes={FileSize} | TempPath={TempPath}",
                    traceId, sessionId, csvFile.FileName, csvFile.Length, originalPath);

                // Transform
                var started = DateTime.UtcNow;
                await using var inputStream = System.IO.File.OpenRead(originalPath);
                using var outputStream = _transformService.Transform(inputStream, options);

                // Save transformed — the service logs the output path.
                var outputFileName = BuildOutputFileName(csvFile.FileName, options.ToLocale);
                var outputPath = await _tempFileService.SaveTransformedAsync(
                    sessionId, outputStream, outputFileName, HttpContext.RequestAborted);

                model.TransformedFileSizeBytes = outputStream.Length;
                var elapsed = DateTime.UtcNow - started;

                _logger.LogInformation(
                    "ConversionSucceeded | TraceId={TraceId} | SessionId={SessionId} | " +
                    "FromLocale={FromLocale} | ToLocale={ToLocale} | " +
                    "InputBytes={InputBytes} | OutputBytes={OutputBytes} | ElapsedMs={ElapsedMs} | OutputPath={OutputPath}",
                    traceId, sessionId, options.FromLocale, options.ToLocale,
                    csvFile.Length, outputStream.Length, (long)elapsed.TotalMilliseconds, outputPath);

                return RenderHomeIndex(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "ConversionFailed | TraceId={TraceId} | SessionId={SessionId} | Error={Error}",
                    traceId, model.SessionId ?? "none", ex.Message);
                model.ErrorMessage = "An error occurred while converting the file. Please check your settings and try again.";
                return RenderHomeIndex(model);
            }
        }

        [HttpGet]
        public IActionResult DownloadOriginal(string sessionId)
        {
            if (!IsValidSessionId(sessionId))
                return BadRequest("Invalid session.");

            var path = _tempFileService.GetOriginalPath(sessionId);
            if (path == null || !System.IO.File.Exists(path))
                return NotFound("Original file not found. It may have been deleted.");

            _logger.LogInformation(
                "FileDownloaded | TraceId={TraceId} | SessionId={SessionId} | FileType=Original | Path={Path}",
                HttpContext.TraceIdentifier, sessionId, path);

            var fileName = Path.GetFileName(path);
            return PhysicalFile(path, "text/csv", fileName);
        }

        [HttpGet]
        public IActionResult DownloadTransformed(string sessionId)
        {
            if (!IsValidSessionId(sessionId))
                return BadRequest("Invalid session.");

            var path = _tempFileService.GetTransformedPath(sessionId);
            if (path == null || !System.IO.File.Exists(path))
                return NotFound("Transformed file not found. It may have been deleted.");

            _logger.LogInformation(
                "FileDownloaded | TraceId={TraceId} | SessionId={SessionId} | FileType=Transformed | Path={Path}",
                HttpContext.TraceIdentifier, sessionId, path);

            var fileName = Path.GetFileName(path);
            return PhysicalFile(path, "text/csv", fileName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cleanup(string sessionId)
        {
            if (!IsValidSessionId(sessionId))
                return BadRequest("Invalid session.");

            _logger.LogInformation(
                "UserDeleteRequested | TraceId={TraceId} | SessionId={SessionId}",
                HttpContext.TraceIdentifier, sessionId);

            _tempFileService.CleanupSession(sessionId);

            _logger.LogInformation(
                "UserDeleteCompleted | TraceId={TraceId} | SessionId={SessionId}",
                HttpContext.TraceIdentifier, sessionId);

            TempData["Message"] = "Your files have been securely deleted.";
            var homeUrl = Url.Action("Index", "Home") ?? "/";
            return Redirect($"{homeUrl}#convert-workspace");
        }

        private IActionResult RenderHomeIndex(ConversionSessionViewModel model) =>
            View("~/Views/Home/Index.cshtml", model);

        private static string BuildOutputFileName(string originalFileName, string toLocale)
        {
            var name = Path.GetFileNameWithoutExtension(originalFileName);
            var ext = Path.GetExtension(originalFileName);
            if (string.IsNullOrEmpty(ext)) ext = ".csv";
            return $"{name}_converted_{toLocale.Replace('-', '_')}{ext}";
        }

        private static bool IsValidSessionId(string? sessionId) =>
            !string.IsNullOrEmpty(sessionId)
            && sessionId.Length == 32
            && sessionId.All(char.IsAsciiLetterOrDigit);
    }
}
