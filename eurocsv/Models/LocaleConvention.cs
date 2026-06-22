namespace eurocsv.Models
{
    public class LocaleConvention
    {
        public string CultureCode { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string FieldDelimiter { get; set; } = ",";
        public string DecimalSeparator { get; set; } = ".";
        public string ThousandSeparator { get; set; } = ",";
        public string TextQualifier { get; set; } = "\"";
        public string DateFormat { get; set; } = "MM/dd/yyyy";
        public string EncodingName { get; set; } = "utf-8";

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
        }.AsReadOnly();

        public static LocaleConvention? FindByCode(string cultureCode) =>
            Presets.FirstOrDefault(p => string.Equals(p.CultureCode, cultureCode, StringComparison.OrdinalIgnoreCase));
    }
}
