using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using eurocsv.Models;

namespace eurocsv.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        // Log the start of a user session.  We use the request TraceIdentifier as the
        // correlation key for this session since there is no authentication.
        var traceId = HttpContext.TraceIdentifier;
        var userAgent = SanitizeForLog(Request.Headers["User-Agent"].ToString());
        var language = SanitizeForLog(Request.Headers["Accept-Language"].ToString());
        _logger.LogInformation(
            "SessionStart | TraceId={TraceId} | UserAgent={UserAgent} | AcceptLanguage={AcceptLanguage}",
            traceId, userAgent, language);

        var model = new ConversionSessionViewModel
        {
            Options = new CsvConversionOptions { FromLocale = "en-GB", ToLocale = "de-DE" },
            AvailableLocales = LocaleConvention.Presets
        };
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    /// <summary>
    /// Sanitizes a string for safe inclusion in log messages by replacing control
    /// characters (CR, LF, TAB, etc.) that could be used for log-forging attacks.
    /// </summary>
    private static string SanitizeForLog(string? value) =>
        string.IsNullOrEmpty(value) ? string.Empty
            : value.Replace('\r', '_').Replace('\n', '_').Replace('\t', '_');
}
