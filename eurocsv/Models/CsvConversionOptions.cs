using System.ComponentModel.DataAnnotations;

namespace eurocsv.Models
{
    /// <summary>
    /// Holds all user-facing settings that control a single CSV conversion run.
    /// </summary>
    /// <remarks>
    /// Each individual separator or delimiter can be overridden via the <c>Custom*</c>
    /// properties; when a custom value is absent the corresponding value from the chosen
    /// <see cref="LocaleConvention"/> preset is used instead (see the <c>Effective*</c> helpers).
    /// </remarks>
    public class CsvConversionOptions
    {
        /// <summary>Gets or sets the BCP-47 culture code of the source locale (e.g. <c>de-DE</c>).</summary>
        [Required]
        public string FromLocale { get; set; } = "en-GB";

        /// <summary>Gets or sets the BCP-47 culture code of the target locale (e.g. <c>en-US</c>).</summary>
        [Required]
        public string ToLocale { get; set; } = "de-DE";

        // Override specific characters; empty means use locale preset
        /// <summary>Optional override for the source field delimiter. Leave blank to use the locale preset.</summary>
        public string? CustomFromDelimiter { get; set; }
        /// <summary>Optional override for the target field delimiter. Leave blank to use the locale preset.</summary>
        public string? CustomToDelimiter { get; set; }

        /// <summary>Optional override for the source decimal separator. Leave blank to use the locale preset.</summary>
        public string? CustomFromDecimal { get; set; }
        /// <summary>Optional override for the target decimal separator. Leave blank to use the locale preset.</summary>
        public string? CustomToDecimal { get; set; }

        /// <summary>Optional override for the source thousands separator. Leave blank to use the locale preset.</summary>
        public string? CustomFromThousands { get; set; }
        /// <summary>Optional override for the target thousands separator. Leave blank to use the locale preset.</summary>
        public string? CustomToThousands { get; set; }

        /// <summary>When <c>true</c>, replaces the source field delimiter with the target delimiter.</summary>
        public bool ConvertDelimiter { get; set; } = true;

        /// <summary>When <c>true</c>, replaces the source decimal separator with the target decimal separator.</summary>
        public bool ConvertDecimalSeparator { get; set; } = true;

        /// <summary>
        /// When <c>true</c>, replaces the source thousands separator with the target thousands separator.
        /// Disabled by default to avoid corrupting date-like values such as <c>31.12.2024</c>.
        /// </summary>
        public bool ConvertThousandSeparator { get; set; } = false;

        /// <summary>
        /// When <c>true</c>, the RFC-4180 quoted-field parser is active:
        /// fields wrapped in <see cref="LocaleConvention.TextQualifier"/> are left untouched
        /// during numeric conversion, and their quotes are preserved or re-added as needed.
        /// </summary>
        public bool HandleTextQualifiers { get; set; } = true;

        /// <summary>When <c>true</c>, all line endings in the output are normalised to <see cref="OutputLineEnding"/>.</summary>
        public bool ConvertLineEndings { get; set; } = false;

        /// <summary>Target line-ending style: <c>"CRLF"</c> (Windows) or <c>"LF"</c> (Unix/Mac). Only used when <see cref="ConvertLineEndings"/> is <c>true</c>.</summary>
        public string OutputLineEnding { get; set; } = "CRLF";

        // Resolved effective settings (derived from locale presets + any overrides)
        /// <summary>Returns the effective source field delimiter: the custom value if set, otherwise the locale preset.</summary>
        public string EffectiveFromDelimiter(LocaleConvention from) =>
            string.IsNullOrEmpty(CustomFromDelimiter) ? from.FieldDelimiter : CustomFromDelimiter;

        /// <summary>Returns the effective target field delimiter: the custom value if set, otherwise the locale preset.</summary>
        public string EffectiveToDelimiter(LocaleConvention to) =>
            string.IsNullOrEmpty(CustomToDelimiter) ? to.FieldDelimiter : CustomToDelimiter;

        /// <summary>Returns the effective source decimal separator: the custom value if set, otherwise the locale preset.</summary>
        public string EffectiveFromDecimal(LocaleConvention from) =>
            string.IsNullOrEmpty(CustomFromDecimal) ? from.DecimalSeparator : CustomFromDecimal;

        /// <summary>Returns the effective target decimal separator: the custom value if set, otherwise the locale preset.</summary>
        public string EffectiveToDecimal(LocaleConvention to) =>
            string.IsNullOrEmpty(CustomToDecimal) ? to.DecimalSeparator : CustomToDecimal;

        /// <summary>Returns the effective source thousands separator: the custom value if set, otherwise the locale preset.</summary>
        public string EffectiveFromThousands(LocaleConvention from) =>
            string.IsNullOrEmpty(CustomFromThousands) ? from.ThousandSeparator : CustomFromThousands;

        /// <summary>Returns the effective target thousands separator: the custom value if set, otherwise the locale preset.</summary>
        public string EffectiveToThousands(LocaleConvention to) =>
            string.IsNullOrEmpty(CustomToThousands) ? to.ThousandSeparator : CustomToThousands;
    }
}
