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
            /*
			Line 1: The HTTP method (POST), followed by a newline.
			Line 2: The request URI (/), followed by a newline.
			Line 3: An empty string. Typically, a query string goes here, but Amazon DynamoDB doesn't use a query string. Follow with a newline
			Line 4-n: The string representing that canonicalized request headers you computed in step 1, followed by a newline.
			The request body. Do not follow the request body with a newline.
			*/
            var sb = new StringBuilder();

            sb.AppendFormat("{0}\n", request.Method);
            sb.AppendFormat("{0}\n", request.RequestUri.AbsolutePath);
            sb.AppendFormat("{0}\n", CanonicizeQueryString(request.RequestUri));
            sb.AppendFormat("{0}\n", CanonicalizeHeaders(request));
            sb.Append("\n");
            sb.Append(request.Content.ReadAsStringAsync().Result);

            return sb.ToString();
        }

        public string CanonicizeQueryString(Uri uri)
        {
            // TODO

            return "";
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

        public byte[] ComputeHash(string text)
        {
            using (var algorithm = SHA256.Create())
            {
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(text));
            }
        }

    }
}