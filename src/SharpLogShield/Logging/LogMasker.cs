using System.Text.RegularExpressions;

namespace SharpLogShield.Logging
{
    public class LogMasker
    {
        private static readonly Regex EmailRegex = new Regex(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}", RegexOptions.Compiled);
        private static readonly Regex CreditCardRegex = new Regex(@"\b(?:\d[ -]*?){13,16}\b", RegexOptions.Compiled);
        private static readonly Regex CodiceFiscaleRegex = new Regex(@"\b[a-zA-Z]{6}[0-9]{2}[a-zA-Z][0-9]{2}[a-zA-Z][0-9]{3}[a-zA-Z]\b", RegexOptions.Compiled);
        private static readonly Regex PhoneNumberRegex = new Regex(@"\b(\+39\s?)?((\d{3}[-.\s]?\d{3}[-.\s]?\d{4})|(\d{2}[-.\s]?\d{4}[-.\s]?\d{4}))\b", RegexOptions.Compiled);
        private static readonly Regex AddressRegex = new Regex(@"\b(?:Via|Viale|Piazza|Corso|Largo|Strada|Piazzale|Vicolo|Borgo)\s+[A-Za-z\s]+,\s?[A-Za-z\s]+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

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
