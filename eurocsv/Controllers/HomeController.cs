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
        var model = new ConversionSessionViewModel
        {
            Options = new CsvConversionOptions { FromLocale = "de-DE", ToLocale = "en-US" },
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
}
