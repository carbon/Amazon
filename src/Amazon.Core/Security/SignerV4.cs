using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;

namespace Amazon.Security
{
    using Helpers;

    public class SignerV4
    {
        const string isoDateTimeFormat = "yyyyMMddTHHmmssZ";  // ISO8601
        const string isoDateFormat     = "yyyyMMdd";

        public static readonly SignerV4 Default = new SignerV4();

        public string GetStringToSign(CredentialScope scope, HttpRequestMessage request)
        {
            #region Preconditions

            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

            string timestamp;

            if (request.Headers.TryGetValues("x-amz-date", out IEnumerable<string> dateHeaderValues))
            {
                timestamp = dateHeaderValues.First();
            }
            else
            { 
                throw new Exception("Missing 'x-amz-date' header");
            }

            return GetStringToSign(
                scope            : scope, 
                timestamp        : timestamp,
                canonicalRequest : GetCanonicalRequest(request)
            );
        }

        public static string GetStringToSign(
            CredentialScope scope, 
            string timestamp,
            string canonicalRequest)
        {
            #region Preconditions

            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            if (timestamp == null)
                throw new ArgumentNullException(nameof(timestamp));

            if (canonicalRequest == null)
                throw new ArgumentNullException(nameof(canonicalRequest));

            #endregion

            var hashedCanonicalRequest = HexString.FromBytes(ComputeSHA256(canonicalRequest));

            return string.Join("\n",
                "AWS4-HMAC-SHA256",     // Algorithm + \n
                timestamp,              // Timestamp + \n
                scope.ToString(),       // Scope     + \n
                hashedCanonicalRequest  // Hex(SHA256(CanonicalRequest))
            );
        }

        // Timestamp format: ISO8601 Basic format, YYYYMMDD'T'HHMMSS'Z'

        public static string GetCanonicalRequest(HttpRequestMessage request)
        {
            return string.Join("\n",
                request.Method,                            // HTTPRequestMethod      + \n
                request.RequestUri.AbsolutePath,           // CanonicalURI           + \n
                CanonicizeQueryString(request.RequestUri), // CanonicalQueryString   + \n
                CanonicalizeHeaders(request),              // CanonicalHeaders       + \n
                string.Empty,                              // \n
                GetSignedHeaders(request),                 // SignedHeaders          + \n
                GetPayloadHash(request)                    // HexEncode(Hash(Payload))
            );
        }

        public static string GetCanonicalRequest(
            HttpMethod method,
            string canonicalURI,
            string canonicalQueryString,
            string canonicalHeaders,
            string signedHeaders,
            string payloadHash
            )
        {
            return string.Join("\n",
                method.ToString(),    // HTTPRequestMethod      + \n
                canonicalURI,         // CanonicalURI           + \n
                canonicalQueryString, // CanonicalQueryString   + \n
                canonicalHeaders,     // CanonicalHeaders       + \n
                string.Empty,         // \n
                signedHeaders,        // SignedHeaders          + \n
                payloadHash           // HexEncode(Hash(Payload))
            );
        }

        // HexEncode(Hash(Payload))
        // If the payload is empty, use an empty string
        private static string GetPayloadHash(HttpRequestMessage request)
        {
            // http://docs.aws.amazon.com/AmazonS3/latest/API/sigv4-streaming.html
            // x-amz-content-sha256:e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b785

            // STREAMING-AWS4-HMAC-SHA256-PAYLOAD
            // UNSIGNED-PAYLOAD

            if (request.Headers.TryGetValues("x-amz-content-sha256", out IEnumerable<string> contentSha256Header))
            {
                return contentSha256Header.First();
            }

            return ComputeSHA256(request.Content);
        }

        public static byte[] GetSigningKey(IAwsCredential credential, CredentialScope scope)
        {
            #region Preconditions

            if (credential == null)
                throw new ArgumentNullException(nameof(credential));

            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            #endregion

            var kSecret = Encoding.ASCII.GetBytes("AWS4" + credential.SecretAccessKey);

            var kDate      = HMACSHA256(kSecret,  scope.Date.ToString("yyyyMMdd"));
            var kRegion    = HMACSHA256(kDate,    scope.Region.Name);
            var kService   = HMACSHA256(kRegion,  scope.Service.Name);
            var signingKey = HMACSHA256(kService, "aws4_request");

            return signingKey;
        }


        // http://docs.aws.amazon.com/general/latest/gr/sigv4-add-signature-to-request.html
        
        public void Presign(
            IAwsCredential credential, 
            CredentialScope scope, 
            DateTime date,
            TimeSpan expires,
            HttpRequestMessage request)
        {
            #region Preconditions

            if (credential == null)
                throw new ArgumentNullException(nameof(credential));

            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

            var signingKey = GetSigningKey(credential, scope);

            var queryParameters = new SortedDictionary<string, string>();

            foreach (var pair in ParseQueryString(request.RequestUri.Query))
            {
                queryParameters[pair.Key] = pair.Value;
            }

            var timestamp = date.ToString(format: isoDateTimeFormat);

            queryParameters["X-Amz-Algorithm"] = "AWS4-HMAC-SHA256";
            queryParameters["X-Amz-Credential"] = $"{credential.AccessKeyId}/{scope}";

            if (credential.SecurityToken != null)
            {
                queryParameters["X-Amz-Security-Token"] = credential.SecurityToken;
            }

            queryParameters["X-Amz-Date"] = timestamp;
            queryParameters["X-Amz-Expires"] = expires.TotalSeconds.ToString(); // in seconds
            queryParameters["X-Amz-SignedHeaders"] = "host";

            var canonicalHeaders = "host:" + request.RequestUri.Host;

            if (!request.RequestUri.IsDefaultPort)
            {
                canonicalHeaders += ":" + request.RequestUri.Port;
            }

            var canonicalRequest = GetCanonicalRequest(
                method               : request.Method,
                canonicalURI         : request.RequestUri.AbsolutePath,
                canonicalQueryString : CanonicizeQueryString(queryParameters),
                canonicalHeaders     : canonicalHeaders,
                signedHeaders        : "host",
                payloadHash          : emptySha256               
            );
            
            var stringToSign = GetStringToSign(
                scope,
                timestamp,
                canonicalRequest
            );
            
            var signature = Signature.ComputeHmacSha256(
                key  : signingKey, 
                data : Encoding.UTF8.GetBytes(stringToSign)
            ).ToHexString();

            /*
            queryString = Action=action
            queryString += &X-Amz-Algorithm=algorithm
            queryString += &X-Amz-Credential= urlencode(access_key_ID + '/' + credential_scope)
            queryString += &X-Amz-Date=date
            queryString += &X-Amz-Expires=timeout interval
            queryString += &X-Amz-SignedHeaders=signed_headers
            */

            var queryString = string.Join("&", 
                queryParameters.Select(pair => WebUtility.UrlEncode(pair.Key) + "=" + WebUtility.UrlEncode(pair.Value))
            ) + "&X-Amz-Signature=" + signature;

            var url = request.RequestUri.ToString();

            request.RequestUri = new Uri(
                url.Substring(0, url.IndexOf("?")) + "?" + queryString
            );
        }

        public void Sign(IAwsCredential credential, CredentialScope scope, HttpRequestMessage request)
        {
            #region Preconditions

            if (credential == null)
                throw new ArgumentNullException(nameof(credential));

            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

            // If we're using S3, ensure the request content has been signed
            if (scope.Service == AwsService.S3 && !request.Headers.Contains("x-amz-content-sha256"))
            {
                request.Headers.Add("x-amz-content-sha256", ComputeSHA256(request.Content));
            }

            var signingKey = GetSigningKey(credential, scope);

            var stringToSign = GetStringToSign(scope, request);

            var signature = Signature.ComputeHmacSha256(signingKey, Encoding.UTF8.GetBytes(stringToSign)).ToHexString();

            var signedHeaders = GetSignedHeaders(request);

            // AWS4-HMAC-SHA256 Credential={0},SignedHeaders={0},Signature={0}
            var auth = $"AWS4-HMAC-SHA256 Credential={credential.AccessKeyId}/{scope},SignedHeaders={signedHeaders},Signature={signature}";

            request.Headers.TryAddWithoutValidation("Authorization", auth);
        }


        // e.g. ?a=1&

        public static string CanonicizeQueryString(Uri uri)
        {
            if (string.IsNullOrEmpty(uri.Query) || uri.Query == "?")
            {
                return string.Empty;
            }
            
            return CanonicizeQueryString(ParseQueryString(uri.Query));
        }

        public static string CanonicizeQueryString(IDictionary<string, string> sortedValues)
        {
            var sb = new StringBuilder();

            // Sort
            foreach (var pair in sortedValues)
            {
                if (sb.Length > 0)
                {
                    sb.Append("&");
                }

                sb.Append(UrlEncoder.Default.Encode(pair.Key));
                sb.Append("=");
                sb.Append(UrlEncoder.Default.Encode(pair.Value));
            }

            return sb.ToString();
        }

        private static IDictionary<string, string> ParseQueryString(string query)
        {
            var dictionary = new SortedDictionary<string, string>();

            if (query[0] == '?')
            {
                query = query.Substring(1);
            }

            foreach (var part in query.Split(Seperators.Ampersand)) // &
            {             
                var split = part.Split(Seperators.Equal); // =

                dictionary[WebUtility.UrlDecode(split[0])] = split.Length == 2 ? WebUtility.UrlDecode(split[1]) : string.Empty;
            }

            return dictionary;
        }

        public static string GetSignedHeaders(HttpRequestMessage request)
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

        public static string CanonicalizeHeaders(HttpRequestMessage request)
        {
            var sb = new StringBuilder();

            if (!request.Headers.Contains("x-amz-date") && request.Headers.Contains("Date"))
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


        #region SHA Helpers

        private const string emptySha256 = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";

        public static byte[] ComputeSHA256(string text)
        {
            using (var algorithm = SHA256.Create())
            {
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(text));
            }
        }

        public static string ComputeSHA256(HttpContent content)
        {
            if (content == null)
            {
                return emptySha256;
            }

            return HexString.FromBytes(ComputeSHA256(content?.ReadAsStringAsync().Result ?? string.Empty));
        }

        private static byte[] HMACSHA256(byte[] key, string data)
        {
            using (var kha = new HMACSHA256(key))
            {
                return kha.ComputeHash(Encoding.UTF8.GetBytes(data));
            }
        }

        #endregion
    }
}
 