namespace eurocsv.Models
{
    /// <summary>
    /// Represents the CSV formatting conventions for a specific locale/region.
    /// </summary>
    /// <remarks>
    /// <para>
    /// EuroCSV ships with 34 built-in presets covering the most common business locales worldwide.
    /// Each preset captures the five formatting conventions that vary between regions:
    /// field delimiter, decimal separator, thousands separator, date format, and file encoding.
    /// </para>
    /// <para>
    /// <strong>Thousands-separator conversion is opt-in</strong> (disabled by default in
    /// <see cref="CsvConversionOptions.ConvertThousandSeparator"/>) because many locales share the
    /// same character for both the thousands separator and the date-component separator
    /// (e.g. <c>31.12.2024</c> in German). Enabling it only when the file genuinely contains
    /// formatted numbers avoids corrupting date-like values.
    /// </para>
    /// </remarks>
    public class LocaleConvention
    {
        /// <summary>Gets or sets the BCP-47 culture code (e.g. <c>de-DE</c>, <c>en-US</c>).</summary>
        public string CultureCode { get; set; } = string.Empty;

        /// <summary>Gets or sets the human-readable locale name shown in the UI.</summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>Gets or sets the character(s) used to separate CSV fields (typically <c>,</c> or <c>;</c>).</summary>
        public string FieldDelimiter { get; set; } = ",";

        /// <summary>Gets or sets the character used as the decimal point (e.g. <c>.</c> for English, <c>,</c> for most of continental Europe).</summary>
        public string DecimalSeparator { get; set; } = ".";

        /// <summary>
        /// Gets or sets the optional character used to group digit groups in large numbers
        /// (e.g. <c>,</c> in <c>1,234.56</c> or <c>.</c> in <c>1.234,56</c>).
        /// An empty string means no thousands separator is used.
        /// </summary>
        public string ThousandSeparator { get; set; } = ",";

        /// <summary>Gets or sets the text qualifier character used to wrap quoted fields (typically <c>"</c>).</summary>
        public string TextQualifier { get; set; } = "\"";

        /// <summary>Gets or sets the conventional date format string for this locale (e.g. <c>dd.MM.yyyy</c>).</summary>
        public string DateFormat { get; set; } = "MM/dd/yyyy";

        /// <summary>Gets or sets the preferred file encoding name for this locale (e.g. <c>utf-8</c>).</summary>
        public string EncodingName { get; set; } = "utf-8";

        /// <summary>
        /// The full set of 34 built-in locale presets, covering:
        /// en-US/GB, de-DE/AT/CH, fr-FR/BE/CH, es-ES/MX, it-IT, nl-NL/BE,
        /// pt-PT/BR, pl-PL, cs-CZ, sk-SK, ru-RU, sv-SE, da-DK, nb-NO, fi-FI,
        /// hu-HU, ro-RO, tr-TR, ja-JP, zh-CN, el-GR, bg-BG, hr-HR, uk-UA, lt-LT, lv-LV.
        /// </summary>
        public static IReadOnlyList<LocaleConvention> Presets { get; } = new List<LocaleConvention>
        {
            new() { CultureCode = "en-US",  DisplayName = "English (United States)",     FieldDelimiter = ",",  DecimalSeparator = ".",  ThousandSeparator = ",",  DateFormat = "MM/dd/yyyy",  EncodingName = "utf-8" },
            new() { CultureCode = "en-GB",  DisplayName = "English (United Kingdom)",    FieldDelimiter = ",",  DecimalSeparator = ".",  ThousandSeparator = ",",  DateFormat = "dd/MM/yyyy",  EncodingName = "utf-8" },
            new() { CultureCode = "de-DE",  DisplayName = "German (Germany)",            FieldDelimiter = ";",  DecimalSeparator = ",",  ThousandSeparator = ".",  DateFormat = "dd.MM.yyyy",  EncodingName = "utf-8" },
            new() { CultureCode = "de-AT",  DisplayName = "German (Austria)",            FieldDelimiter = ";",  DecimalSeparator = ",",  ThousandSeparator = ".",  DateFormat = "dd.MM.yyyy",  EncodingName = "utf-8" },
            new() { CultureCode = "de-CH",  DisplayName = "German (Switzerland)",        FieldDelimiter = ";",  DecimalSeparator = ".",  ThousandSeparator = "'",  DateFormat = "dd.MM.yyyy",  EncodingName = "utf-8" },
            new() { CultureCode = "fr-FR",  DisplayName = "French (France)",             FieldDelimiter = ";",  DecimalSeparator = ",",  ThousandSeparator = " ",  DateFormat = "dd/MM/yyyy",  EncodingName = "utf-8" },
            new() { CultureCode = "fr-BE",  DisplayName = "French (Belgium)",            FieldDelimiter = ";",  DecimalSeparator = ",",  ThousandSeparator = ".",  DateFormat = "dd/MM/yyyy",  EncodingName = "utf-8" },
            new() { CultureCode = "fr-CH",  DisplayName = "French (Switzerland)",        FieldDelimiter = ";",  DecimalSeparator = ".",  ThousandSeparator = "'",  DateFormat = "dd.MM.yyyy",  EncodingName = "utf-8" },
            new() { CultureCode = "es-ES",  DisplayName = "Spanish (Spain)",             FieldDelimiter = ";",  DecimalSeparator = ",",  ThousandSeparator = ".",  DateFormat = "dd/MM/yyyy",  EncodingName = "utf-8" },
            new() { CultureCode = "es-MX",  DisplayName = "Spanish (Mexico)",            FieldDelimiter = ",",  DecimalSeparator = ".",  ThousandSeparator = ",",  DateFormat = "dd/MM/yyyy",  EncodingName = "utf-8" },
            new() { CultureCode = "it-IT",  DisplayName = "Italian (Italy)",             FieldDelimiter = ";",  DecimalSeparator = ",",  ThousandSeparator = ".",  DateFormat = "dd/MM/yyyy",  EncodingName = "utf-8" },
            new() { CultureCode = "nl-NL",  DisplayName = "Dutch (Netherlands)",         FieldDelimiter = ";",  DecimalSeparator = ",",  ThousandSeparator = ".",  DateFormat = "dd-MM-yyyy",  EncodingName = "utf-8" },
            new() { CultureCode = "nl-BE",  DisplayName = "Dutch (Belgium)",             FieldDelimiter = ";",  DecimalSeparator = ",",  ThousandSeparator = ".",  DateFormat = "dd/MM/yyyy",  EncodingName = "utf-8" },
            new() { CultureCode = "pt-PT",  DisplayName = "Portuguese (Portugal)",       FieldDelimiter = ";",  DecimalSeparator = ",",  ThousandSeparator = ".",  DateFormat = "dd/MM/yyyy",  EncodingName = "utf-8" },
            new() { CultureCode = "pt-BR",  DisplayName = "Portuguese (Brazil)",         FieldDelimiter = ";",  DecimalSeparator = ",",  ThousandSeparator = ".",  DateFormat = "dd/MM/yyyy",  EncodingName = "utf-8" },
            new() { CultureCode = "pl-PL",  DisplayName = "Polish (Poland)",             FieldDelimiter = ";",  DecimalSeparator = ",",  ThousandSeparator = " ",  DateFormat = "dd.MM.yyyy",  EncodingName = "utf-8" },
            new() { CultureCode = "cs-CZ",  DisplayName = "Czech (Czech Republic)",      FieldDelimiter = ";",  DecimalSeparator = ",",  ThousandSeparator = " ",  DateFormat = "dd.MM.yyyy",  EncodingName = "utf-8" },
            new() { CultureCode = "sk-SK",  DisplayName = "Slovak (Slovakia)",           FieldDelimiter = ";",  DecimalSeparator = ",",  ThousandSeparator = " ",  DateFormat = "dd.MM.yyyy",  EncodingName = "utf-8" },
            new() { CultureCode = "ru-RU",  DisplayName = "Russian (Russia)",            FieldDelimiter = ";",  DecimalSeparator = ",",  ThousandSeparator = " ",  DateFormat = "dd.MM.yyyy",  EncodingName = "utf-8" },
            new() { CultureCode = "sv-SE",  DisplayName = "Swedish (Sweden)",            FieldDelimiter = ";",  DecimalSeparator = ",",  ThousandSeparator = " ",  DateFormat = "yyyy-MM-dd",  EncodingName = "utf-8" },
            new() { CultureCode = "da-DK",  DisplayName = "Danish (Denmark)",            FieldDelimiter = ";",  DecimalSeparator = ",",  ThousandSeparator = ".",  DateFormat = "dd-MM-yyyy",  EncodingName = "utf-8" },
            new() { CultureCode = "nb-NO",  DisplayName = "Norwegian (Norway)",          FieldDelimiter = ";",  DecimalSeparator = ",",  ThousandSeparator = " ",  DateFormat = "dd.MM.yyyy",  EncodingName = "utf-8" },
            new() { CultureCode = "fi-FI",  DisplayName = "Finnish (Finland)",           FieldDelimiter = ";",  DecimalSeparator = ",",  ThousandSeparator = " ",  DateFormat = "dd.MM.yyyy",  EncodingName = "utf-8" },
            new() { CultureCode = "hu-HU",  DisplayName = "Hungarian (Hungary)",         FieldDelimiter = ";",  DecimalSeparator = ",",  ThousandSeparator = " ",  DateFormat = "yyyy.MM.dd",  EncodingName = "utf-8" },
            new() { CultureCode = "ro-RO",  DisplayName = "Romanian (Romania)",          FieldDelimiter = ";",  DecimalSeparator = ",",  ThousandSeparator = ".",  DateFormat = "dd.MM.yyyy",  EncodingName = "utf-8" },
            new() { CultureCode = "tr-TR",  DisplayName = "Turkish (Turkey)",            FieldDelimiter = ";",  DecimalSeparator = ",",  ThousandSeparator = ".",  DateFormat = "dd.MM.yyyy",  EncodingName = "utf-8" },
            new() { CultureCode = "ja-JP",  DisplayName = "Japanese (Japan)",            FieldDelimiter = ",",  DecimalSeparator = ".",  ThousandSeparator = ",",  DateFormat = "yyyy/MM/dd",  EncodingName = "utf-8" },
            new() { CultureCode = "zh-CN",  DisplayName = "Chinese Simplified (China)",  FieldDelimiter = ",",  DecimalSeparator = ".",  ThousandSeparator = ",",  DateFormat = "yyyy/MM/dd",  EncodingName = "utf-8" },
            new() { CultureCode = "el-GR",  DisplayName = "Greek (Greece)",              FieldDelimiter = ";",  DecimalSeparator = ",",  ThousandSeparator = ".",  DateFormat = "dd/MM/yyyy",  EncodingName = "utf-8" },
            new() { CultureCode = "bg-BG",  DisplayName = "Bulgarian (Bulgaria)",        FieldDelimiter = ";",  DecimalSeparator = ",",  ThousandSeparator = " ",  DateFormat = "dd.MM.yyyy",  EncodingName = "utf-8" },
            new() { CultureCode = "hr-HR",  DisplayName = "Croatian (Croatia)",          FieldDelimiter = ";",  DecimalSeparator = ",",  ThousandSeparator = ".",  DateFormat = "dd.MM.yyyy",  EncodingName = "utf-8" },
            new() { CultureCode = "uk-UA",  DisplayName = "Ukrainian (Ukraine)",         FieldDelimiter = ";",  DecimalSeparator = ",",  ThousandSeparator = " ",  DateFormat = "dd.MM.yyyy",  EncodingName = "utf-8" },
            new() { CultureCode = "lt-LT",  DisplayName = "Lithuanian (Lithuania)",      FieldDelimiter = ";",  DecimalSeparator = ",",  ThousandSeparator = " ",  DateFormat = "yyyy-MM-dd",  EncodingName = "utf-8" },
            new() { CultureCode = "lv-LV",  DisplayName = "Latvian (Latvia)",            FieldDelimiter = ";",  DecimalSeparator = ",",  ThousandSeparator = " ",  DateFormat = "dd.MM.yyyy",  EncodingName = "utf-8" },
        }.AsReadOnly();

        /// <summary>
        /// Looks up a preset by its BCP-47 culture code (case-insensitive).
        /// Returns <c>null</c> if no matching preset exists.
        /// </summary>
        public static LocaleConvention? FindByCode(string cultureCode) =>
            Presets.FirstOrDefault(p => string.Equals(p.CultureCode, cultureCode, StringComparison.OrdinalIgnoreCase));
    }
}
