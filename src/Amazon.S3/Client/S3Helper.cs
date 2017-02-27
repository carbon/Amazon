using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace Amazon.S3
{
    public class S3Helper
    {
        private static readonly Dictionary<string, string> emptyStringDictionary = new Dictionary<string, string>();

        public static string GetSignedUrl(GetUrlRequest request, IAwsCredentials credentials)
        {
            // You can specify any future expiration time in epoch or UNIX time (number of seconds since January 1, 1970).

            var unixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            long expires = unixTime + (long)request.ExpiresIn.TotalSeconds;

            var stringToSign = ConstructStringToSign(
                httpVerb      : HttpMethod.Get,
                contentType   : string.Empty,
                bucketName    : request.BucketName,
                key           : request.Key,
                headers       : emptyStringDictionary,
                query         : string.Empty,
                expiresOrDate : expires.ToString()
            );

            var signature = ComputeSignature(credentials.SecretAccessKey, stringToSign);

            return new StringBuilder()
                .Append("https://")
                .Append(request.BucketName)
                .Append(".s3.amazonaws.com/")
                .Append(request.Key)
                .Append("?AWSAccessKeyId=")
                .Append(credentials.AccessKeyId.UrlEncodeX2())
                .Append("&Expires=")
                .Append(expires)
                .Append("&Signature=")
                .Append(signature.UrlEncodeX2())
                .ToString();
        }

        private static string ConstructStringToSign(HttpMethod httpVerb, string contentType, string bucketName, string key,
            Dictionary<string, string> headers, string query, string expiresOrDate)
        {
            #region Preconditions

            if (httpVerb == null)
                throw new ArgumentNullException("httpVerb");

            if (headers == null)
                throw new ArgumentNullException("headers");

            #endregion

            var sb = new StringBuilder();

            sb.Append(httpVerb.ToString()).Append("\n");              // HTTP-VERB + \n + 
            sb.Append("\n");                                          // Content-MD5 + \n + 
            sb.Append(contentType).Append("\n");                      // Content-Type+ \n + 
            sb.Append(expiresOrDate).Append("\n");                    // Expires/ Date + \n + 
            sb.Append(string.Empty);                                  // CanonicalizedAmzHeaders
            sb.Append(CanonicalizeResource(bucketName, key, query));  // CanonicalizedResource

            return sb.ToString();
        }

        private static string CanonicalizeResource(string bucketName, string key, string query)
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

        private static string ComputeSignature(string secret, string stringToSign)
        {
            #region Preconditions

            if (secret == null)
                throw new ArgumentNullException(nameof(secret));

            if (stringToSign == null)
                throw new ArgumentNullException(nameof(stringToSign));

            #endregion

            // Signature = Base64(HMAC-SHA1(UTF-8-Encoding-Of(StringToSign)));

            using (var hmacSha1 = new HMACSHA1(Encoding.UTF8.GetBytes(secret)))
            {
                var hashBytes = hmacSha1.ComputeHash(Encoding.UTF8.GetBytes(stringToSign.ToCharArray()));

                return Convert.ToBase64String(hashBytes);
            }
        }
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
