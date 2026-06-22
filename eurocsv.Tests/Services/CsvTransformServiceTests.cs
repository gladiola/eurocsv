using eurocsv.Models;
using eurocsv.Services;
using System.Text;
using Xunit;

namespace eurocsv.Tests.Services
{
    public class CsvTransformServiceTests
    {
        private readonly CsvTransformService _service = new();

        private static CsvConversionOptions DefaultOptions(string from, string to) => new()
        {
            FromLocale = from,
            ToLocale = to,
            ConvertDelimiter = true,
            ConvertDecimalSeparator = true,
            ConvertThousandSeparator = false,
            HandleTextQualifiers = true
        };

        private static string TransformString(CsvTransformService svc, string input, CsvConversionOptions options)
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(input));
            using var result = svc.Transform(stream, options);
            return Encoding.UTF8.GetString(result.ToArray()).TrimEnd('\r', '\n');
        }

        // ── Delimiter conversion ──────────────────────────────────────────────

        [Fact]
        public void Transform_GermanToEnglish_ConvertsDelimiter()
        {
            var options = DefaultOptions("de-DE", "en-US");
            var result = TransformString(_service, "Name;Wert;Datum", options);
            Assert.Equal("Name,Wert,Datum", result);
        }

        [Fact]
        public void Transform_EnglishToGerman_ConvertsDelimiter()
        {
            var options = DefaultOptions("en-US", "de-DE");
            var result = TransformString(_service, "Name,Value,Date", options);
            Assert.Equal("Name;Value;Date", result);
        }

        // ── Decimal separator conversion ──────────────────────────────────────

        [Fact]
        public void Transform_GermanToEnglish_ConvertsDecimalSeparator()
        {
            var options = DefaultOptions("de-DE", "en-US");
            var result = TransformString(_service, "Name;Preis\nArtikel A;1234,56", options);
            var lines = result.Split('\n');
            Assert.Equal("Name,Preis", lines[0].TrimEnd('\r'));
            Assert.Equal("Artikel A,1234.56", lines[1].TrimEnd('\r'));
        }

        [Fact]
        public void Transform_EnglishToGerman_ConvertsDecimalSeparator()
        {
            var options = DefaultOptions("en-US", "de-DE");
            var result = TransformString(_service, "Name,Price\nItem A,1234.56", options);
            var lines = result.Split('\n');
            Assert.Equal("Name;Price", lines[0].TrimEnd('\r'));
            Assert.Equal("Item A;1234,56", lines[1].TrimEnd('\r'));
        }

        // ── Quoted fields ─────────────────────────────────────────────────────

        [Fact]
        public void Transform_QuotedField_PreservesContent()
        {
            var options = DefaultOptions("de-DE", "en-US");
            // Quoted field containing a semicolon — the semicolon inside quotes should not be split
            var result = TransformString(_service, "\"Müller, GmbH\";1234,00", options);
            Assert.Equal("\"Müller, GmbH\",1234.00", result);
        }

        [Fact]
        public void Transform_QuotedFieldWithEscapedQuote_Preserved()
        {
            var options = DefaultOptions("de-DE", "en-US");
            var result = TransformString(_service, "\"Say \"\"hello\"\"\";42,0", options);
            Assert.Equal("\"Say \"\"hello\"\"\",42.0", result);
        }

        // ── Thousands separator ───────────────────────────────────────────────

        [Fact]
        public void Transform_ThousandsSeparator_Disabled_NoConversion()
        {
            var options = DefaultOptions("de-DE", "en-US");
            options.ConvertThousandSeparator = false;
            // The dot in "1.234,56" is the German thousands separator; without enabling thousands conversion, only decimal changes
            var result = TransformString(_service, "Wert\n1.234,56", options);
            var lines = result.Split('\n');
            Assert.Equal("1.234.56", lines[1].TrimEnd('\r'));
        }

        [Fact]
        public void Transform_ThousandsSeparator_Enabled_Converts()
        {
            var options = DefaultOptions("de-DE", "en-US");
            options.ConvertThousandSeparator = true;
            // German: 1.234,56 → English: 1,234.56
            // The result 1,234.56 contains a comma (the target delimiter) so it must be quoted in valid CSV
            var result = TransformString(_service, "Wert\n1.234,56", options);
            var lines = result.Split('\n');
            Assert.Equal("\"1,234.56\"", lines[1].TrimEnd('\r'));
        }

        // ── Multiple lines ────────────────────────────────────────────────────

        [Fact]
        public void Transform_MultipleLines_AllConverted()
        {
            var options = DefaultOptions("de-DE", "en-US");
            var input = "Name;Preis\nArtikel A;10,99\nArtikel B;999,00";
            var result = TransformString(_service, input, options);
            var lines = result.Split('\n').Select(l => l.TrimEnd('\r')).ToArray();
            Assert.Equal("Name,Preis", lines[0]);
            Assert.Equal("Artikel A,10.99", lines[1]);
            Assert.Equal("Artikel B,999.00", lines[2]);
        }

        // ── Custom overrides ──────────────────────────────────────────────────

        [Fact]
        public void Transform_CustomDelimiter_UsesCustom()
        {
            var options = DefaultOptions("de-DE", "en-US");
            options.CustomFromDelimiter = "|";
            options.CustomToDelimiter = "\t";
            var result = TransformString(_service, "Name|Value", options);
            Assert.Equal("Name\tValue", result);
        }

        // ── Unknown locale ────────────────────────────────────────────────────

        [Fact]
        public void Transform_UnknownFromLocale_ThrowsArgumentException()
        {
            var options = DefaultOptions("xx-XX", "en-US");
            using var stream = new MemoryStream("a;b"u8.ToArray());
            Assert.Throws<ArgumentException>(() => _service.Transform(stream, options));
        }

        [Fact]
        public void Transform_UnknownToLocale_ThrowsArgumentException()
        {
            var options = DefaultOptions("de-DE", "xx-XX");
            using var stream = new MemoryStream("a;b"u8.ToArray());
            Assert.Throws<ArgumentException>(() => _service.Transform(stream, options));
        }

        // ── SplitCsvLine unit tests ───────────────────────────────────────────

        [Fact]
        public void SplitCsvLine_Simple_ReturnsTwoFields()
        {
            var fields = CsvTransformService.SplitCsvLine("a;b", ";", "\"", handleQualifiers: true);
            Assert.Equal(2, fields.Count);
            Assert.Equal("a", fields[0].Value);
            Assert.Equal("b", fields[1].Value);
        }

        [Fact]
        public void SplitCsvLine_QuotedField_ReturnsQuotedTrue()
        {
            var fields = CsvTransformService.SplitCsvLine("\"hello world\";plain", ";", "\"", handleQualifiers: true);
            Assert.Equal(2, fields.Count);
            Assert.True(fields[0].WasQuoted);
            Assert.Equal("hello world", fields[0].Value);
            Assert.False(fields[1].WasQuoted);
        }

        [Fact]
        public void SplitCsvLine_EscapedQuoteInField_ReturnsUnescapedValue()
        {
            var fields = CsvTransformService.SplitCsvLine("\"say \"\"hi\"\"\"", ";", "\"", handleQualifiers: true);
            Assert.Single(fields);
            Assert.Equal("say \"hi\"", fields[0].Value);
        }

        [Fact]
        public void SplitCsvLine_DelimiterInsideQuotes_NotSplit()
        {
            var fields = CsvTransformService.SplitCsvLine("\"a;b\";c", ";", "\"", handleQualifiers: true);
            Assert.Equal(2, fields.Count);
            Assert.Equal("a;b", fields[0].Value);
            Assert.Equal("c", fields[1].Value);
        }

        // ── LocaleConvention lookup ───────────────────────────────────────────

        [Fact]
        public void LocaleConvention_FindByCode_ReturnsCorrectPreset()
        {
            var de = LocaleConvention.FindByCode("de-DE");
            Assert.NotNull(de);
            Assert.Equal(";", de.FieldDelimiter);
            Assert.Equal(",", de.DecimalSeparator);
        }

        [Fact]
        public void LocaleConvention_FindByCode_CaseInsensitive()
        {
            var enUs = LocaleConvention.FindByCode("EN-US");
            Assert.NotNull(enUs);
            Assert.Equal(",", enUs.FieldDelimiter);
        }

        [Fact]
        public void LocaleConvention_FindByCode_UnknownReturnsNull()
        {
            Assert.Null(LocaleConvention.FindByCode("xx-XX"));
        }

        [Fact]
        public void LocaleConvention_Presets_HasAtLeastTenEntries()
        {
            Assert.True(LocaleConvention.Presets.Count >= 10);
        }
    }
}
