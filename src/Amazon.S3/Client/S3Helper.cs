using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace Amazon.S3
{
    public class S3Helper
    {
        public const string AmazonMetadataHeaderPrefix = "x-amz-meta-";
        public const string AmazonHeaderPrefix = "x-amz-";

        public static string ConstructStringToSign(HttpMethod httpVerb, string contentType, string bucketName, string key,
            Dictionary<string, string> headers, string query, string expiresOrDate)
        {
            #region Preconditions

            if (httpVerb == null)
                throw new ArgumentNullException("httpVerb");

            if (headers == null)
                throw new ArgumentNullException("headers");

            #endregion

            var sb = new StringBuilder();

            sb.AppendFormat("{0}\n", httpVerb.ToString());                                              // HTTP-VERB + \n + 
            sb.AppendFormat("{0}\n", headers.ContainsKey("Content-MD5") ? headers["Content-MD5"] : ""); // Content-MD5 + \n + 
            sb.AppendFormat("{0}\n", contentType);                                                      // Content-Type+ \n + 
            sb.AppendFormat("{0}\n", expiresOrDate);                                                    // Expires/ Date + \n + 
            sb.Append(CanonicalizeAmzHeaders(headers));                                                 // CanonicalizedAmzHeaders
            sb.Append(CanonicalizeResource(bucketName, key, query));									// CanonicalizedResource

            return sb.ToString();
        }

        public static string CanonicalizeAmzHeaders(Dictionary<string, string> headers)
        {
            // To construct the CanonicalizedAmzHeaders part of StringToSign, select all HTTP request headers that start with 'x-amz-' 
            // (using a case-insensitive comparison) and use the following process. 

            // 1. Convert each HTTP header name to lower-case. For example, 'X-Amz-Date' becomes 'x-amz-date'.

            // 2. Sort the collection of headers lexicographically by header name.

            // 3. Combine header fields with the same name into one "header-name:comma-separated-value-list" 
            // pair as prescribed by RFC 2616, section 4.2, without any white-space between values. 
            // For example, the two metadata headers 'x-amz-meta-username: fred' and 'x-amz-meta-username: barney' 
            // would be combined into the single header 'x-amz-meta-username: fred,barney'.

            // 4. "Un-fold" long headers that span multiple lines (as allowed by RFC 2616, section 4.2) 
            // by replacing the folding white-space (including new-line) by a single space.

            // 5. Trim any white-space around the colon in the header. 
            // For example, the header 'x-amz-meta-username: fred,barney' would become 'x-amz-meta-username:fred,barney'

            // 6. Finally, append a new-line (U+000A) to each canonicalized header in the resulting list. 
            // Construct the CanonicalizedResource element by concatenating all headers in this list into a single string.

            var amazonHeaders = headers
                .Where(pair => pair.Key.StartsWith("x-amz-", StringComparison.OrdinalIgnoreCase))
                .OrderBy(pair => pair.Key);

            var sb = new StringBuilder();

            foreach (var pair in amazonHeaders)
            {
                sb.Append(pair.Key.ToLower());
                sb.Append(":");
                sb.Append(pair.Value);
                sb.Append("\n");
            }

            return sb.ToString();
        }

        public static string CanonicalizeResource(string bucketName, string key, string query)
        {
            #region Preconditions

            if (bucketName == null)
                throw new ArgumentNullException("bucketName");

            #endregion

            // 1. Start with the empty string

            // 2. If the request specifies a bucket using the HTTP Host header (virtual hosted-style), 
            // append the bucket name preceded by a "/" (e.g., "/bucketname"). For path-style requests 
            // and requests that don't address a bucket, do nothing.

            // 3. Append the path part of the un-decoded HTTP Request-URI, up-to but not including the query string.

            // 4. If the request addresses a sub-resource, like ?location, ?acl, or ?torrent, append the sub-resource including question mark. 

            var sb = new StringBuilder();

            sb.Append("/");
            sb.Append(bucketName);
            sb.Append("/");

            if (key != null)
            {
                sb.Append(key);
            }

            if (query != null)
            {
                if (query.Contains("acl"))
                {
                    sb.Append("?acl");
                }
                else if (query.Contains("torrent"))
                {
                    sb.Append("?torrent");
                }
                else if (query.Contains("logging"))
                {
                    sb.Append("?logging");
                }
            }

            return sb.ToString();
        }

        public static string ComputeSignature(string secret, string stringToSign)
        {
            #region Preconditions

            if (secret == null) throw new ArgumentNullException("secret");

            if (stringToSign == null) throw new ArgumentNullException("stringToSign");

            #endregion

            // Signature = Base64(HMAC-SHA1(UTF-8-Encoding-Of(StringToSign)));

            using (var hmacSha1 = new HMACSHA1(Encoding.UTF8.GetBytes(secret)))
            {
                var hashBytes = hmacSha1.ComputeHash(Encoding.UTF8.GetBytes(stringToSign.ToCharArray()));

                return Convert.ToBase64String(hashBytes);
            }
        }

        public static long GetSecondsSince1970()
            => DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        public static long GetMillisecondsSince1970()
            => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}


/*
On signing.

The first few header elements of StringToSign (Content-Type, Date, and Content-MD5) are positional in nature. 
StringToSign does not include the names of these headers, only their values from the request.
In contrast, the 'x-amz-' elements are named; Both the header names and the header values appear in StringToSign. 

A valid time-stamp (using either the HTTP Date header or an x-amz-date alternative) is mandatory for authenticated requests. 
Furthermore, the client time-stamp included with an authenticated request must be within 15 minutes of the Amazon S3 system 
time when the request is received. If not, the request will fail with the RequestTimeTooSkewed error status code. 
The intention of these restrictions is to limit the possibility that intercepted requests could be replayed by an adversary. 
For stronger protection against eavesdropping, use the HTTPS transport for authenticated requests. 

Some HTTP client libraries do not expose the ability to set the Date header for a request. 
If you have trouble including the value of the 'Date' header in the canonicalized headers, 
you can set the time-stamp for the request using an 'x-amz-date' header instead. 
The value of the x-amz-date header must be in one of the RFC 2616 formats (http://www.ietf.org/rfc/rfc2616.txt). 
When an x-amz-date header is present in a request, the system will ignore any Date header when computing the request signature. 
Therefore, if you include the x-amz-date  header, use the empty string for the Date when constructing the StringToSign. 
*/
