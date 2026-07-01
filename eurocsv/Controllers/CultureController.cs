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
                HttpContext.TraceIdentifier, culture);

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), IsEssential = true, Secure = true, SameSite = SameSiteMode.Strict }
            );

            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                return RedirectToAction("Index", "Home");

            return LocalRedirect(returnUrl);
        }
    }
}
