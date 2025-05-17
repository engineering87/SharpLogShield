// (c) 2025 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using System.Text.RegularExpressions;

namespace SharpLogShield.Logging
{
    /// <summary>
    /// Utility class that provides masking functionality for sensitive data in log messages.
    /// </summary>
    public class LogMasker
    {
        // Regex to identify email addresses.
        private static readonly Regex EmailRegex = new Regex(
            @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}",
            RegexOptions.Compiled);

        // Regex to identify potential credit card numbers (13 to 16 digits).
        private static readonly Regex CreditCardRegex = new Regex(
            @"\b(?:\d[ -]*?){13,16}\b",
            RegexOptions.Compiled);

        // Regex to identify Italian Codice Fiscale (fiscal code).
        private static readonly Regex CodiceFiscaleRegex = new Regex(
            @"\b[a-zA-Z]{6}[0-9]{2}[a-zA-Z][0-9]{2}[a-zA-Z][0-9]{3}[a-zA-Z]\b",
            RegexOptions.Compiled);

        // Regex to identify Italian-style phone numbers.
        private static readonly Regex PhoneNumberRegex = new Regex(
            @"\b(\+39\s?)?((\d{3}[-.\s]?\d{3}[-.\s]?\d{4})|(\d{2}[-.\s]?\d{4}[-.\s]?\d{4}))\b",
            RegexOptions.Compiled);

        // Regex to identify Italian addresses starting with common keywords.
        private static readonly Regex AddressRegex = new Regex(
            @"\b(?:Via|Viale|Piazza|Corso|Largo|Strada|Piazzale|Vicolo|Borgo)\s+[A-Za-z\s]+,\s?[A-Za-z\s]+\b",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Masks sensitive data (e.g., emails, credit card numbers, fiscal codes, phone numbers, addresses)
        /// in the given log message using predefined regular expressions.
        /// </summary>
        /// <param name="message">The original log message.</param>
        /// <returns>The log message with sensitive data masked.</returns>
        public static string MaskSensitiveData(string message)
        {
            if (string.IsNullOrEmpty(message)) return message;

            message = EmailRegex.Replace(message, m => $"{m.Groups[1].Value}*****{m.Groups[3].Value}");
            message = CreditCardRegex.Replace(message, m => $"{m.Groups[1].Value} **** **** {m.Groups[2].Value}");
            message = CodiceFiscaleRegex.Replace(message, m => $"{m.Groups[1].Value}********{m.Groups[2].Value}");
            message = PhoneNumberRegex.Replace(message, m => $"{(m.Groups[1].Success ? m.Groups[1].Value : "")}{m.Groups[2].Value}-***-{m.Groups[3].Value}");
            message = AddressRegex.Replace(message, m => $"{m.Groups[0].Value.Substring(0, 8)}***{m.Groups[3].Value}");

            return message;
        }
    }
}
