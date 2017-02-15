using System;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace Amazon.Security
{
    public abstract class SignerV3
    {
        public string GenerateStringToSign(HttpRequestMessage request)
        {
            return string.Join(/*seperator*/ "\n",
                request.Method,                             // Line 1   : HTTP method (POST) + \n
                request.RequestUri.AbsolutePath,            // Line 2   : Request URI (/)    + \n
                CanonicizeQueryString(request.RequestUri),  // Line 3   : Querystring        + \n
                CanonicalizeHeaders(request),               // Line 4-n : Headers            + \n
                "",                                         // newline                       + \n
                request.Content.ReadAsStringAsync().Result  // Request body
            );

            /*
            var sb = new StringBuilder();

            sb.Append(request.Method)                               .Append("\n");
            sb.Append(request.RequestUri.AbsolutePath)              .Append("\n");
            sb.Append(CanonicizeQueryString(request.RequestUri))    .Append("\n");
            sb.Append(CanonicalizeHeaders(request))                 .Append("\n");
            sb.Append("\n");
            sb.Append(request.Content.ReadAsStringAsync().Result);

            return sb.ToString();
            */
        }

        public string CanonicizeQueryString(Uri uri)
        {
            // TODO

            return string.Empty;
        }


        public string GetSignedHeaders(HttpRequestMessage request)
        {
            // Sonvert all header names to lowercase
            // Sort them by character code
            // Use a semicolon to separate the header names

            // The host header must be included as a signed header.

            var signedHeaders = "host;" + string.Join(";", request.Headers
                .Select(item => item.Key.ToLower())
                .Where(key => key.StartsWith("x-amz-"))
                .OrderBy(k => k));

            if (!signedHeaders.Contains("x-amz-date"))
            {
                signedHeaders = "date;" + signedHeaders;
            }

            return signedHeaders;
        }

        public string CanonicalizeHeaders(HttpRequestMessage request)
        {
            var sb = new StringBuilder();

            if (!request.Headers.Contains("x-amz-date"))
            {
                sb.Append("date:");
                sb.Append(request.Headers.GetValues("Date").First());
                sb.Append("\n");
            }

            sb.Append("host:").Append(request.Headers.Host);

            foreach (var header in request.Headers
                                            .Where(item => item.Key.StartsWith("x-amz-", StringComparison.OrdinalIgnoreCase))
                                            .OrderBy(item => item.Key.ToLower()))
            {
                sb.Append("\n");

                sb.Append(header.Key.ToLower());
                sb.Append(":");
                sb.Append(string.Join(";", header.Value));
            }

            return sb.ToString();
        }

        protected byte[] ComputeHash(string text)
        {
            using (var algorithm = SHA256.Create())
            {
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(text));
            }
        }
    }
}