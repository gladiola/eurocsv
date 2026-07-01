using System.Globalization;
using eurocsv;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace eurocsv.Tests.Localization
{
    public class SharedResourceLocalizationTests
    {
        private static IStringLocalizer<SharedResource> CreateLocalizer()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            return services
                .BuildServiceProvider()
                .GetRequiredService<IStringLocalizer<SharedResource>>();
        }

        [Fact]
        public void SharedResource_ResolvesGermanStrings()
        {
            var localizer = CreateLocalizer();

            using var _ = new CultureScope("de-DE");
            var value = localizer["LanguageSelect"];

            Assert.False(value.ResourceNotFound);
            Assert.Equal("Sprache", value.Value);
        }

        [Fact]
        public void SharedResource_FallsBackToParentCulture()
        {
            var localizer = CreateLocalizer();

            using var _ = new CultureScope("pt-PT");
            var value = localizer["ConvertUploadSection"];

            Assert.False(value.ResourceNotFound);
            Assert.Equal("Enviar e converter", value.Value);
        }

        private sealed class CultureScope : IDisposable
        {
            private readonly CultureInfo _originalCulture = CultureInfo.CurrentCulture;
            private readonly CultureInfo _originalUICulture = CultureInfo.CurrentUICulture;

            public CultureScope(string cultureName)
            {
                var culture = CultureInfo.GetCultureInfo(cultureName);
                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;
            }

            public void Dispose()
            {
                CultureInfo.CurrentCulture = _originalCulture;
                CultureInfo.CurrentUICulture = _originalUICulture;
            }
        }
    }
}
