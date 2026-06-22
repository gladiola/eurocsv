using System.ComponentModel.DataAnnotations;

namespace eurocsv.Models
{
    public class CsvConversionOptions
    {
        [Required]
        public string FromLocale { get; set; } = "de-DE";

        [Required]
        public string ToLocale { get; set; } = "en-US";

        // Override specific characters; empty means use locale preset
        public string? CustomFromDelimiter { get; set; }
        public string? CustomToDelimiter { get; set; }

        public string? CustomFromDecimal { get; set; }
        public string? CustomToDecimal { get; set; }

        public string? CustomFromThousands { get; set; }
        public string? CustomToThousands { get; set; }

        public bool ConvertDelimiter { get; set; } = true;
        public bool ConvertDecimalSeparator { get; set; } = true;
        public bool ConvertThousandSeparator { get; set; } = false;
        public bool HandleTextQualifiers { get; set; } = true;
        public bool ConvertLineEndings { get; set; } = false;

        public string OutputLineEnding { get; set; } = "CRLF";

        // Resolved effective settings (derived from locale presets + any overrides)
        public string EffectiveFromDelimiter(LocaleConvention from) =>
            string.IsNullOrEmpty(CustomFromDelimiter) ? from.FieldDelimiter : CustomFromDelimiter;

        public string EffectiveToDelimiter(LocaleConvention to) =>
            string.IsNullOrEmpty(CustomToDelimiter) ? to.FieldDelimiter : CustomToDelimiter;

        public string EffectiveFromDecimal(LocaleConvention from) =>
            string.IsNullOrEmpty(CustomFromDecimal) ? from.DecimalSeparator : CustomFromDecimal;

        public string EffectiveToDecimal(LocaleConvention to) =>
            string.IsNullOrEmpty(CustomToDecimal) ? to.DecimalSeparator : CustomToDecimal;

        public string EffectiveFromThousands(LocaleConvention from) =>
            string.IsNullOrEmpty(CustomFromThousands) ? from.ThousandSeparator : CustomFromThousands;

        public string EffectiveToThousands(LocaleConvention to) =>
            string.IsNullOrEmpty(CustomToThousands) ? to.ThousandSeparator : CustomToThousands;
    }
}
