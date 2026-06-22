namespace eurocsv.Models
{
    public class ConversionSessionViewModel
    {
        public string SessionId { get; set; } = string.Empty;
        public string OriginalFileName { get; set; } = string.Empty;
        public long OriginalFileSizeBytes { get; set; }
        public long TransformedFileSizeBytes { get; set; }
        public CsvConversionOptions Options { get; set; } = new();
        public LocaleConvention? FromConvention { get; set; }
        public LocaleConvention? ToConvention { get; set; }
        public IReadOnlyList<LocaleConvention> AvailableLocales { get; set; } = LocaleConvention.Presets;
        public string? ErrorMessage { get; set; }
    }
}
