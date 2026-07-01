using eurocsv.Services;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddLocalization();
builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();
builder.Services.AddAntiforgery();
builder.Services.AddScoped<ICsvTransformService, CsvTransformService>();
builder.Services.AddSingleton<ITempFileService, TempFileService>();
builder.Services.AddHostedService<SessionPurgeService>();

// Configure upload size limit (50 MB)
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(o =>
{
    o.MultipartBodyLengthLimit = 50 * 1024 * 1024;
});
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 50 * 1024 * 1024;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

// All 34 locale presets, grouped by unique UI language so ASP.NET Core fallback chain works.
// Region-specific cultures (e.g. de-DE, de-AT, de-CH) inherit translations from the parent
// culture file (SharedResource.de.resx); no separate per-region UI file is required.
var supportedCultures = new[]
{
    "en-US", "en-GB",
    "de-DE", "de-AT", "de-CH",
    "fr-FR", "fr-BE", "fr-CH",
    "es-ES", "es-MX",
    "it-IT",
    "nl-NL", "nl-BE",
    "pt-PT", "pt-BR",
    "pl-PL",
    "cs-CZ",
    "sk-SK",
    "ru-RU",
    "sv-SE",
    "da-DK",
    "nb-NO",
    "fi-FI",
    "hu-HU",
    "ro-RO",
    "tr-TR",
    "ja-JP",
    "zh-CN",
    "el-GR",
    "bg-BG",
    "hr-HR",
    "uk-UA",
    "lt-LT",
    "lv-LV",
};

app.UseRequestLocalization(new RequestLocalizationOptions()
    .SetDefaultCulture("de-DE")
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures));

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

// Make Program accessible for testing
public partial class Program { }

