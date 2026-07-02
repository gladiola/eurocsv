using eurocsv.Middleware;
using eurocsv.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Serilog;
using Serilog.Events;

// Configure Serilog before the host is built so startup errors are also captured.
// Logs rotate hourly and are retained for 30 days (720 files × 1 h = 30 days).
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(
        path: Path.Combine("logs", "eurocsv-.log"),
        rollingInterval: RollingInterval.Hour,
        retainedFileCountLimit: 720,          // 30 days × 24 h/day
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}",
        shared: false,
        flushToDiskInterval: TimeSpan.FromSeconds(5))
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

// Replace the default logging with Serilog.
builder.Host.UseSerilog((ctx, services, loggerConfig) =>
    loggerConfig
        .ReadFrom.Configuration(ctx.Configuration)
        .ReadFrom.Services(services)
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .WriteTo.Console(
            outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
        .WriteTo.File(
            path: Path.Combine("logs", "eurocsv-.log"),
            rollingInterval: RollingInterval.Hour,
            retainedFileCountLimit: 720,
            outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}",
            shared: false,
            flushToDiskInterval: TimeSpan.FromSeconds(5)));

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

// Trust X-Forwarded-For and X-Forwarded-Proto from a reverse proxy (e.g. nginx).
// Must be called before UseHttpsRedirection so that the forwarded scheme is visible
// to subsequent middleware and redirect logic works correctly behind TLS termination.
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

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

// Security headers and nonce/CSP must run before static files and routing
// so that every response — including 4xx short-circuits — carries them.
app.UseMiddleware<SecurityHeadersMiddleware>();
app.UseMiddleware<NonceMiddleware>();

// Log each request with its method, path, status code, and duration.
app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate =
        "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms | TraceId={TraceId} | ClientIp={ClientIp}";
    options.GetLevel = (ctx, elapsed, ex) =>
        ex != null || ctx.Response.StatusCode >= 500
            ? LogEventLevel.Error
            : LogEventLevel.Information;
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("TraceId", httpContext.TraceIdentifier);
        diagnosticContext.Set("ClientIp", httpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty);
    };
});

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

