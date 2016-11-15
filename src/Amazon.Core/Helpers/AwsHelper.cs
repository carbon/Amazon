using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Net.Http;

namespace Amazon
{
    public static class AwsHelper
    {
        public static string ComputeWebRequestSignature(string key, HttpRequestMessage webRequest, IDictionary<string, string> parameters)
        {
            var stringToSign = ConstructStringToSign(webRequest, parameters);

            return Signature.ComputeHmacSha256(key, stringToSign).ToBase64String();
        }

        public static string ConstructStringToSign(HttpRequestMessage request, IDictionary<string, string> parameters)
        {
            /* ----------------------------------------------------------------------------------------
			1. The HTTP Request Method followed by an ASCII newline (%0A)
			2. The HTTP Host header in the form of lowercase host, followed by an ASCII newline.
			3. The URL encoded HTTP absolute path component of the URI
			   (up to but not including the query string parameters);
			   if this is empty use a forward '/'. This parameter is followed by an ASCII newline.
			4. The concatenation of all query string components (names and values)
			   as UTF-8 characters which are URL encoded as per RFC 3986
			   (hex characters MUST be uppercase), sorted using lexicographic byte ordering.
			   Parameter names are separated from their values by the '=' character
			   (ASCII character 61), even if the value is empty.
			   Pairs of parameter and values are separated by the '&' character (ASCII code 38).
			---------------------------------------------------------------------------------------- */

           return new StringBuilder()
              .Append(request.Method.ToString().ToUpper())    .Append("\n")   // HttpVerb + \n + 
              .Append(request.RequestUri.Authority.ToLower()) .Append("\n")   // ValueOfHostHeaderInLowercase + \n + 
              .Append(request.RequestUri.AbsolutePath)        .Append("\n")   // HttpRequestURI + \n +
              .Append(CanonicalizeParameters(parameters))                     // CanonicalizedQueryString
              .ToString();
        }

        public static string CanonicalizeParameters(IDictionary<string, string> parameters)
        {
            /* ----------------------------------------------------------------------------------------
			Sort the UTF-8 components by parameter name with natural byte ordering.

			URL encode the parameter name and values according to the following rules:
			* Do not URL encode any of the unreserved characters that RFC 3986 defines.
			* These unreserved characters are A-Z, a-z, 0-9, hyphen ( - ), underscore ( _ ), period ( . ), and tilde ( ~ ).
			* Percent encode all other characters with %XY, where X and Y are hex characters 0-9 and uppercase A-F.
			* Percent encode extended UTF-8 characters in the form %XY%ZA....
			* Percent encode the space character as %20 (and not +, as common encoding schemes do).
			
			Separate the encoded parameter names from their encoded values with the equals sign ( = ) (ASCII character 61),
			even if the parameter value is empty.
			
			Separate the name-value pairs with an ampersand ( & ) (ASCII code 38).
			---------------------------------------------------------------------------------------- */

            var sortedParameters = new SortedDictionary<string, string>(parameters, StringComparer.Ordinal);

            return ConvertParametersToString(sortedParameters);
        }

        public static string ConvertParametersToString(IDictionary<string, string> parameters)
        {
            var sb = new StringBuilder();

            foreach (var pair in parameters)
            {
                if (pair.Value == null) continue;

                if (sb.Length > 0)
                {
                    sb.Append("&");
                }

                // {key}={value}
                sb.Append(pair.Key.UrlEncodeX2());
                sb.Append("=");
                sb.Append(pair.Value.UrlEncodeX2());
            }

            return sb.ToString();
        }

        // ISO 8601 timestamp: i.e. 2009-12-20T19:23:18Z
        public static string GetFormattedTimestamp()
            => DateTime.UtcNow.ToString("s", CultureInfo.InvariantCulture) + "Z";
    }
}