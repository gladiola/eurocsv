namespace eurocsv.Services
{
    public interface ITempFileService
    {
        /// <summary>
        /// Saves an uploaded file to a temporary session directory.
        /// Returns a new session ID (GUID string).
        /// </summary>
        Task<(string SessionId, string FilePath)> SaveUploadAsync(IFormFile file, CancellationToken ct = default);

        /// <summary>
        /// Writes a transformed stream to the session directory.
        /// Returns the path to the output file.
        /// </summary>
        Task<string> SaveTransformedAsync(string sessionId, Stream content, string outputFileName, CancellationToken ct = default);

        /// <summary>
        /// Returns the path to the original uploaded file for a given session.
        /// Returns null if the session does not exist.
        /// </summary>
        string? GetOriginalPath(string sessionId);

        /// <summary>
        /// Returns the path to the transformed file for a given session.
        /// Returns null if the session does not exist.
        /// </summary>
        string? GetTransformedPath(string sessionId);

        /// <summary>
        /// Deletes all files for a session. Safe to call even if the session does not exist.
        /// </summary>
        void CleanupSession(string sessionId);

        /// <summary>
        /// Removes all sessions older than the specified age.
        /// </summary>
        void PurgeExpiredSessions(TimeSpan maxAge);
    }

    public class TempFileService : ITempFileService
    {
        private readonly string _basePath;
        private readonly ILogger<TempFileService> _logger;

        private const string OriginalSubdir = "original";
        private const string TransformedSubdir = "transformed";

        public TempFileService(ILogger<TempFileService> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            // Store temp files outside wwwroot so they are never served statically
            _basePath = Path.Combine(Path.GetTempPath(), "eurocsv_sessions");
            Directory.CreateDirectory(_basePath);
        }

        public async Task<(string SessionId, string FilePath)> SaveUploadAsync(IFormFile file, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(file);

            var sessionId = Guid.NewGuid().ToString("N");
            var sessionDir = GetSessionDirectory(sessionId);
            Directory.CreateDirectory(Path.Combine(sessionDir, OriginalSubdir));

            var safeFileName = SanitizeFileName(file.FileName);
            var filePath = Path.Combine(sessionDir, OriginalSubdir, safeFileName);

            await using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            await file.CopyToAsync(fs, ct);

            _logger.LogInformation("Saved upload for session {SessionId}: {FileName}", sessionId, safeFileName);
            return (sessionId, filePath);
        }

        public async Task<string> SaveTransformedAsync(string sessionId, Stream content, string outputFileName, CancellationToken ct = default)
        {
            ValidateSessionId(sessionId);

            var sessionDir = GetSessionDirectory(sessionId);
            Directory.CreateDirectory(Path.Combine(sessionDir, TransformedSubdir));

            var safeFileName = SanitizeFileName(outputFileName);
            var filePath = Path.Combine(sessionDir, TransformedSubdir, safeFileName);

            await using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            await content.CopyToAsync(fs, ct);

            return filePath;
        }

        public string? GetOriginalPath(string sessionId)
        {
            ValidateSessionId(sessionId);
            var dir = Path.Combine(GetSessionDirectory(sessionId), OriginalSubdir);
            if (!Directory.Exists(dir)) return null;
            return Directory.EnumerateFiles(dir).FirstOrDefault();
        }

        public string? GetTransformedPath(string sessionId)
        {
            ValidateSessionId(sessionId);
            var dir = Path.Combine(GetSessionDirectory(sessionId), TransformedSubdir);
            if (!Directory.Exists(dir)) return null;
            return Directory.EnumerateFiles(dir).FirstOrDefault();
        }

        public void CleanupSession(string sessionId)
        {
            ValidateSessionId(sessionId);
            var sessionDir = GetSessionDirectory(sessionId);
            if (Directory.Exists(sessionDir))
            {
                Directory.Delete(sessionDir, recursive: true);
                _logger.LogInformation("Cleaned up session {SessionId}", sessionId);
            }
        }

        public void PurgeExpiredSessions(TimeSpan maxAge)
        {
            if (!Directory.Exists(_basePath)) return;

            var cutoff = DateTime.UtcNow - maxAge;
            foreach (var dir in Directory.EnumerateDirectories(_basePath))
            {
                try
                {
                    var created = Directory.GetCreationTimeUtc(dir);
                    if (created < cutoff)
                    {
                        Directory.Delete(dir, recursive: true);
                        _logger.LogInformation("Purged expired session directory: {Dir}", Path.GetFileName(dir));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to purge session directory: {Dir}", dir);
                }
            }
        }

        private string GetSessionDirectory(string sessionId) =>
            Path.Combine(_basePath, sessionId);

        private static string SanitizeFileName(string fileName)
        {
            var name = Path.GetFileName(fileName);
            var invalid = Path.GetInvalidFileNameChars();
            var sanitized = new string(name.Select(c => invalid.Contains(c) ? '_' : c).ToArray());
            return string.IsNullOrWhiteSpace(sanitized) ? "upload.csv" : sanitized;
        }

        private static void ValidateSessionId(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId) || sessionId.Length != 32
                || !sessionId.All(char.IsAsciiLetterOrDigit))
            {
                throw new ArgumentException("Invalid session ID.", nameof(sessionId));
            }
        }
    }
}
