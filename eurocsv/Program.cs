using eurocsv.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();
builder.Services.AddAntiforgery();
builder.Services.AddScoped<ICsvTransformService, CsvTransformService>();
builder.Services.AddSingleton<ITempFileService, TempFileService>();

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

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Purge expired sessions on startup (older than 2 hours)
using (var scope = app.Services.CreateScope())
{
    var tempFileService = scope.ServiceProvider.GetRequiredService<ITempFileService>();
    tempFileService.PurgeExpiredSessions(TimeSpan.FromHours(2));
}

app.Run();

// Make Program accessible for testing
public partial class Program { }

