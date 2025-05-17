// (c) 2025 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using System.Text.RegularExpressions;

namespace SharpLogShield.Logging
{
    /// <summary>
    /// Utility class that provides masking functionality for sensitive data in log messages.
    /// Applies masking per-type without invalidare le regex tramite sostituzioni a cascata.
    /// </summary>
    public class LogMasker
    {
        private static readonly Regex EmailRegex = new Regex(
            @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}",
            RegexOptions.Compiled);

        private static readonly Regex CreditCardRegex = new Regex(
            @"\b(?:\d[ -]*?){13,16}\b",
            RegexOptions.Compiled);

        private static readonly Regex CodiceFiscaleRegex = new Regex(
            @"\b[a-zA-Z]{6}[0-9]{2}[a-zA-Z][0-9]{2}[a-zA-Z][0-9]{3}[a-zA-Z]\b",
            RegexOptions.Compiled);

        private static readonly Regex PhoneNumberRegex = new Regex(
            @"\b(\+39\s?)?((\d{3}[-.\s]?\d{3}[-.\s]?\d{4})|(\d{2}[-.\s]?\d{4}[-.\s]?\d{4}))\b",
            RegexOptions.Compiled);

        private static readonly Regex AddressRegex = new Regex(
            @"\b(?:Via|Viale|Piazza|Corso|Largo|Strada|Piazzale|Vicolo|Borgo)\s+[A-Za-z\s]+,\s?[A-Za-z\s]+\b",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Masks sensitive data (emails, credit cards, fiscal codes, phone numbers, addresses) in the given message.
        /// The masking preserves some parts visible depending on data type.
        /// </summary>
        public static string MaskSensitiveData(string message)
        {
            if (string.IsNullOrEmpty(message))
                return message;

            var replacements = new List<(int start, int length, string replacement)>();

            // Collect matches and mask replacements for each pattern:
            AddReplacements(message, EmailRegex, replacements, MaskEmail);
            AddReplacements(message, CreditCardRegex, replacements, MaskCreditCard);
            AddReplacements(message, CodiceFiscaleRegex, replacements, MaskCodiceFiscale);
            AddReplacements(message, PhoneNumberRegex, replacements, MaskPhoneNumber);
            AddReplacements(message, AddressRegex, replacements, MaskAddress);

            // Sort replacements by start index descending to apply without messing indices
            replacements.Sort((a, b) => b.start.CompareTo(a.start));

            string maskedMessage = message;
            foreach (var r in replacements)
            {
                maskedMessage = maskedMessage.Remove(r.start, r.length)
                                             .Insert(r.start, r.replacement);
            }

            return maskedMessage;
        }

        private static void AddReplacements(string text, Regex regex, List<(int, int, string)> replacements, Func<string, string> maskFunc)
        {
            foreach (Match match in regex.Matches(text))
            {
                string replacement = maskFunc(match.Value);
                replacements.Add((match.Index, match.Length, replacement));
            }
        }

        private static string MaskEmail(string email)
        {
            var parts = email.Split('@');
            if (parts.Length != 2) return "***@***";
            return $"***@{parts[1]}";
        }

        private static string MaskCreditCard(string cc)
        {
            // Remove spaces and dashes, keep last 4 digits visible
            string digits = Regex.Replace(cc, @"[^\d]", "");
            if (digits.Length < 4) return "**** **** **** ****";

            string last4 = digits.Substring(digits.Length - 4);
            return $"**** **** **** {last4}";
        }

        private static string MaskCodiceFiscale(string codice)
        {
            if (codice.Length != 16) return "***************";

            // Keep first 3 and last 4 visible, mask the rest
            return codice.Substring(0, 3) + new string('*', 9) + codice.Substring(12, 4);
        }

        private static string MaskPhoneNumber(string phone)
        {
            // This regex match groups: optional +39, then number parts
            // We'll preserve prefix (area code) and last 2 digits, mask middle part

            // Clean input to digits and symbols for safety
            string digits = Regex.Replace(phone, @"[^\d]", "");
            if (digits.Length < 4) return "***-***-****";

            // Try to detect prefix (area code)
            // For Italian style numbers 10 or 11 digits (with +39)
            // We'll preserve the first 3 digits and last 2 digits for masking

            string prefix = digits.Substring(0, 3);
            string suffix = digits.Substring(digits.Length - 2);
            return $"{prefix}-***-**{suffix}";
        }

        private static string MaskAddress(string address)
        {
            // Preserve the address type (e.g., Via) and city, mask the street name
            // Assumes pattern: <Type> <StreetName>, <City>
            // Mask street name characters with '*'

            var parts = address.Split(',', 2);
            if (parts.Length < 2) return "***MASKED ADDRESS***";

            string typeAndStreet = parts[0];
            string city = parts[1].Trim();

            // Extract type (first word) and rest street name
            var words = typeAndStreet.Split(' ', 2);
            if (words.Length < 2) return "***MASKED ADDRESS***";

            string type = words[0];
            string streetName = words[1];

            string maskedStreet = new string('*', streetName.Length);

            return $"{type} {maskedStreet}, {city}";
        }
    }
}