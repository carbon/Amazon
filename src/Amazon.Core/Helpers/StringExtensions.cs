using System;
using System.Text.Encodings.Web;

namespace Amazon
{
    public static class StringExtensions
    {
        internal static TEnum ToEnum<TEnum>(this string self)
        {
            #region Preconditions

            if (self == null)
                throw new ArgumentNullException("self");

            #endregion

            return (TEnum)Enum.Parse(typeof(TEnum), self);
        }

        // Percent encode all unreserved characters
        // The standard HttpUtility.UrlEncode uses x2 (lowercase)

        public static string UrlEncodeX2(this string text)
            => string.IsNullOrEmpty(text) ? text : UrlEncoder.Default.Encode(text);
    }
}