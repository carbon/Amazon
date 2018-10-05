using System;
using System.Net.Mail;
using System.Text;

namespace Amazon.Ses
{
    public static class SesHelper
    {
        public static string EncodeEmail(MailAddress email)
            => EncodeEmail(email?.ToString());

        public static string EncodeEmail(string email)
        {
            if (email is null)
                throw new ArgumentNullException(nameof(email));

            // By default, the string must be 7-bit ASCII. 
            // If the text must contain any other characters, 
            // then you must use MIME encoded-word syntax (RFC 2047) instead of a literal string.
            // MIME encoded-word syntax uses the following form: =?charset?encoding?encoded-text?=. For more information, see RFC 2047.

            var encoded = Encode(email);

            if (encoded != email)
            {
                return $"=?UTF-8?Q?{encoded}?=";

                // "=?charset?encoding?encoded-text?=";
            }

            return email;
        }

        private static string Encode(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;

            var sb = new StringBuilder();

            byte[] bytes = Encoding.UTF8.GetBytes(value);

            foreach (byte v in bytes)
            {
                // The following are not required to be encoded:
                // - Tab (ASCII 9)
                // - Space (ASCII 32)
                // - Characters 33 to 126, except for the equal sign (61).

                if ((v == 9) || ((v >= 32) && (v <= 60)) || ((v >= 62) && (v <= 126)))
                {
                    sb.Append(Convert.ToChar(v));
                }
                else
                {
                    sb.Append('=');
                    sb.Append(v.ToString("X2"));
                }
            }

            char lastChar = sb[sb.Length - 1];

            if (char.IsWhiteSpace(lastChar))
            {
                sb.Remove(sb.Length - 1, 1);
                sb.Append('=');
                sb.Append(((int)lastChar).ToString("X2"));
            }

            return sb.ToString();
        }
    }
}