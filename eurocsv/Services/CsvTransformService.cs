using eurocsv.Models;
using System.Text;

namespace eurocsv.Services
{
    /// <summary>
    /// Defines the contract for transforming a CSV stream between two locale conventions.
    /// </summary>
    public interface ICsvTransformService
    {
        /// <summary>
        /// Transforms a CSV stream using the provided options.
        /// Returns a MemoryStream positioned at offset 0 with the transformed content.
        /// </summary>
        MemoryStream Transform(Stream inputStream, CsvConversionOptions options);
    }

    /// <summary>
    /// Core CSV transformation engine.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <strong>Field splitting</strong> — implements the RFC-4180 quoted-field parser when
    /// <see cref="CsvConversionOptions.HandleTextQualifiers"/> is <c>true</c>.  Escaped
    /// double-quotes inside a quoted field (represented as <c>""</c>) are handled correctly;
    /// the de-escaped value is used for numeric conversion and the quotes are re-applied on
    /// output.  Values that acquire the target delimiter after conversion are automatically
    /// re-quoted.
    /// </para>
    /// <para>
    /// <strong>Numeric conversion</strong> — uses a three-step placeholder swap to prevent
    /// double-replacement when the decimal separator and the thousands separator share the same
    /// character in one of the two locales (e.g. converting <c>1.234,56</c> DE→EN):
    /// <list type="number">
    ///   <item>Replace the <em>source</em> thousands separator with an internal placeholder (<c>\x01\x02\x03</c>).</item>
    ///   <item>Replace the <em>source</em> decimal separator with the target decimal separator.</item>
    ///   <item>Replace the placeholder with the <em>target</em> thousands separator.</item>
    /// </list>
    /// </para>
    /// <para>
    /// <strong>Thousands-separator opt-in</strong> — thousands conversion is only active when
    /// <see cref="CsvConversionOptions.ConvertThousandSeparator"/> is explicitly set to
    /// <c>true</c>.  The default is <c>false</c> to avoid corrupting date-like values such as
    /// <c>31.12.2024</c> that happen to contain the same character as the thousands separator.
    /// </para>
    /// </remarks>
    public class CsvTransformService : ICsvTransformService
    {
        public MemoryStream Transform(Stream inputStream, CsvConversionOptions options)
        {
            ArgumentNullException.ThrowIfNull(inputStream);
            ArgumentNullException.ThrowIfNull(options);

            var fromLocale = LocaleConvention.FindByCode(options.FromLocale)
                ?? throw new ArgumentException($"Unknown source locale: {options.FromLocale}");
            var toLocale = LocaleConvention.FindByCode(options.ToLocale)
                ?? throw new ArgumentException($"Unknown target locale: {options.ToLocale}");

            var fromDelim = options.EffectiveFromDelimiter(fromLocale);
            var toDelim = options.EffectiveToDelimiter(toLocale);
            var fromDecimal = options.EffectiveFromDecimal(fromLocale);
            var toDecimal = options.EffectiveToDecimal(toLocale);
            var fromThousands = options.EffectiveFromThousands(fromLocale);
            var toThousands = options.EffectiveToThousands(toLocale);
            var qualifier = fromLocale.TextQualifier;

            // Use UTF-8 without BOM so the output is compatible with most tools
            var encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
            var output = new MemoryStream();
            using var reader = new StreamReader(inputStream, encoding, detectEncodingFromByteOrderMarks: true, leaveOpen: true);
            using var writer = new StreamWriter(output, encoding, leaveOpen: true);

            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                var transformed = TransformLine(line, fromDelim, toDelim, fromDecimal, toDecimal,
                    fromThousands, toThousands, qualifier, options);

                if (options.ConvertLineEndings)
                {
                    if (options.OutputLineEnding == "LF")
                        writer.Write(transformed + "\n");
                    else
                        writer.Write(transformed + "\r\n");
                }
                else
                {
                    writer.WriteLine(transformed);
                }
            }

            writer.Flush();
            output.Position = 0;
            return output;
        }

        private static string TransformLine(
            string line,
            string fromDelim,
            string toDelim,
            string fromDecimal,
            string toDecimal,
            string fromThousands,
            string toThousands,
            string qualifier,
            CsvConversionOptions options)
        {
            if (!options.ConvertDelimiter && !options.ConvertDecimalSeparator && !options.ConvertThousandSeparator)
                return line;

            var fields = SplitCsvLine(line, fromDelim, qualifier, options.HandleTextQualifiers);
            var transformedFields = new string[fields.Count];

            for (int i = 0; i < fields.Count; i++)
            {
                var (rawValue, wasQuoted) = fields[i];
                var value = rawValue;

                if (!wasQuoted)
                {
                    value = TransformNumericValue(value, fromDecimal, toDecimal, fromThousands, toThousands, options);
                }

                if (options.ConvertDelimiter)
                {
                    // Re-quote if the value now contains the target delimiter
                    bool needsQuoting = options.HandleTextQualifiers
                        && (value.Contains(toDelim) || value.Contains(qualifier) || value.Contains('\n'));
                    if (needsQuoting)
                    {
                        value = qualifier + value.Replace(qualifier, qualifier + qualifier) + qualifier;
                    }
                    else if (wasQuoted && options.HandleTextQualifiers)
                    {
                        value = qualifier + value + qualifier;
                    }
                }
                else if (wasQuoted && options.HandleTextQualifiers)
                {
                    value = qualifier + value + qualifier;
                }

                transformedFields[i] = value;
            }

            return string.Join(options.ConvertDelimiter ? toDelim : fromDelim, transformedFields);
        }

        /// <summary>
        /// Applies decimal and/or thousands separator replacements to a single unquoted field value,
        /// using a placeholder to prevent characters from being replaced twice.
        /// </summary>
        private static string TransformNumericValue(
            string value,
            string fromDecimal,
            string toDecimal,
            string fromThousands,
            string toThousands,
            CsvConversionOptions options)
        {
            // Only attempt numeric transforms when separators differ
            bool decimalDiffers = options.ConvertDecimalSeparator && fromDecimal != toDecimal;
            bool thousandsDiffers = options.ConvertThousandSeparator && fromThousands != toThousands;

            if (!decimalDiffers && !thousandsDiffers)
                return value;

            // Use a placeholder to avoid double-replacement
            const string placeholder = "\x01\x02\x03";

            string result = value;

            if (thousandsDiffers && !string.IsNullOrEmpty(fromThousands))
            {
                result = result.Replace(fromThousands, placeholder);
            }

            if (decimalDiffers && !string.IsNullOrEmpty(fromDecimal))
            {
                result = result.Replace(fromDecimal, toDecimal);
            }

            if (thousandsDiffers)
            {
                result = result.Replace(placeholder, toThousands);
            }

            return result;
        }

        /// <summary>
        /// Splits a CSV line respecting quoted fields.
        /// Returns a list of (rawValue, wasQuoted) tuples.
        /// When handleQualifiers is false, simply splits on the delimiter.
        /// </summary>
        internal static List<(string Value, bool WasQuoted)> SplitCsvLine(
            string line,
            string delimiter,
            string qualifier,
            bool handleQualifiers)
        {
            var fields = new List<(string, bool)>();

            if (!handleQualifiers || string.IsNullOrEmpty(qualifier))
            {
                foreach (var f in line.Split(delimiter))
                    fields.Add((f, false));
                return fields;
            }

            char q = qualifier[0];
            int pos = 0;

            while (pos <= line.Length)
            {
                if (pos < line.Length && line[pos] == q)
                {
                    // Quoted field
                    var sb = new StringBuilder();
                    pos++; // skip opening quote
                    while (pos < line.Length)
                    {
                        if (line[pos] == q)
                        {
                            if (pos + 1 < line.Length && line[pos + 1] == q)
                            {
                                // Escaped quote
                                sb.Append(q);
                                pos += 2;
                            }
                            else
                            {
                                pos++; // skip closing quote
                                break;
                            }
                        }
                        else
                        {
                            sb.Append(line[pos++]);
                        }
                    }
                    fields.Add((sb.ToString(), true));
                    // Skip delimiter
                    if (pos < line.Length && line.Substring(pos).StartsWith(delimiter))
                        pos += delimiter.Length;
                    else
                        break;
                }
                else
                {
                    // Unquoted field
                    int delimIdx = line.IndexOf(delimiter, pos, StringComparison.Ordinal);
                    if (delimIdx < 0)
                    {
                        fields.Add((line.Substring(pos), false));
                        break;
                    }
                    else
                    {
                        fields.Add((line.Substring(pos, delimIdx - pos), false));
                        pos = delimIdx + delimiter.Length;
                    }
                }
            }

            return fields;
        }
    }
}
