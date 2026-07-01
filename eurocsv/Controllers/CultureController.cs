using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace eurocsv.Controllers
{
    /// <summary>
    /// Handles language/culture switching by writing the ASP.NET Core culture cookie.
    /// </summary>
    public class CultureController : Controller
    {
        private readonly ILogger<CultureController> _logger;

        public CultureController(ILogger<CultureController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetCulture(string culture, string returnUrl)
        {
            _logger.LogInformation(
                "LanguageSelected | TraceId={TraceId} | Culture={Culture}",
                HttpContext.TraceIdentifier, SanitizeForLog(culture));

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), IsEssential = true, Secure = true, SameSite = SameSiteMode.Strict }
            );

            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                return RedirectToAction("Index", "Home");

            return LocalRedirect(returnUrl);
        }

        /// <summary>
        /// Sanitizes a string for safe inclusion in log messages by replacing control
        /// characters (CR, LF, TAB, etc.) that could be used for log-forging attacks.
        /// </summary>
        private static string SanitizeForLog(string? value) =>
            string.IsNullOrEmpty(value) ? string.Empty
                : value.Replace('\r', '_').Replace('\n', '_').Replace('\t', '_');
    }
}
