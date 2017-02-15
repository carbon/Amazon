using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;

namespace Amazon.Security
{
    using Helpers;

    public class SignerV4
    {
        public static readonly SignerV4 Default = new SignerV4();

        public string GetStringToSign(CredentialScope scope, HttpRequestMessage request)
        {
            #region Preconditions

            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

            IEnumerable<string> dateHeaderValues;

            if (!request.Headers.TryGetValues("x-amz-date", out dateHeaderValues))
            {
                throw new Exception("Missing x-amz-date header");
            }

            var timestamp = dateHeaderValues.First();
            var canonicalRequest = HexString.FromBytes(ComputeSHA256(GetCanonicalRequest(request)));

            return string.Join("\n",
                "AWS4-HMAC-SHA256", // Algorithm + \n
                timestamp,          // Timestamp + \n
                scope.ToString(),   // Scope     + \n
                canonicalRequest    // Hex(SHA256(CanonicalRequest))
            );
        }

        // Timestamp format: ISO8601 Basic format, YYYYMMDD'T'HHMMSS'Z'

        public string GetCanonicalRequest(HttpRequestMessage request)
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
                
            /*
            return new StringBuilder()
                .Append(request.Method)                             .Append("\n") // HTTPRequestMethod      + \n
                .Append(request.RequestUri.AbsolutePath)            .Append("\n") // CanonicalURI           + \n
                .Append(CanonicizeQueryString(request.RequestUri))  .Append("\n") // CanonicalQueryString   + \n
                .Append(CanonicalizeHeaders(request))               .Append("\n") // CanonicalHeaders       + \n
                .Append("\n")                                                     // \n
                .Append(GetSignedHeaders(request))                  .Append("\n") // SignedHeaders          + \n
                .Append(GetPayloadHash(request))                                  // HexEncode(Hash(Payload))
                .ToString();
            */
        }

        // HexEncode(Hash(Payload))
        // If the payload is empty, use an empty string
        public string GetPayloadHash(HttpRequestMessage request)
        {
            IEnumerable<string> contentSha256Header;

            // http://docs.aws.amazon.com/AmazonS3/latest/API/sigv4-streaming.html
            // x-amz-content-sha256:e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b785

            // STREAMING-AWS4-HMAC-SHA256-PAYLOAD
            // UNSIGNED-PAYLOAD

            if (request.Headers.TryGetValues("x-amz-content-sha256", out contentSha256Header))
            {
                var hash = contentSha256Header.First();

                return hash;              
            }

            return ComputeSHA256(request.Content);
        }

        public static string ComputeSHA256(HttpContent content)
        {
            if (content == null) return "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";

            return HexString.FromBytes(ComputeSHA256(content?.ReadAsStringAsync().Result ?? string.Empty));
        }

        private static byte[] HMACSHA256(byte[] key, string data)
        {
            using (var kha = new HMACSHA256(key))
            {
                return kha.ComputeHash(Encoding.UTF8.GetBytes(data));
            }
        }

        public byte[] GetSigningKey(IAwsCredentials credentials, CredentialScope scope)
        {
            #region Preconditions

            if (credentials == null) throw new ArgumentNullException(nameof(credentials));
            if (scope == null) throw new ArgumentNullException(nameof(scope));

            #endregion

            var kSecret = Encoding.ASCII.GetBytes("AWS4" + credentials.SecretAccessKey);

            var kDate = HMACSHA256(kSecret, scope.Date.ToString("yyyyMMdd"));
            var kRegion = HMACSHA256(kDate, scope.Region.Name);
            var kService = HMACSHA256(kRegion, scope.Service.Name);
            var signingKey = HMACSHA256(kService, "aws4_request");

            return signingKey;
        }

        public void Sign(IAwsCredentials credentials, CredentialScope scope, HttpRequestMessage request)
        {
            #region Preconditions

            if (credentials == null)  throw new ArgumentNullException(nameof(credentials));
            if (scope == null)        throw new ArgumentNullException(nameof(scope));
            if (request == null)      throw new ArgumentNullException(nameof(request));

            #endregion

            // If we're using S3, ensure the request content has been signed
            if (scope.Service == AwsService.S3 && !request.Headers.Contains("x-amz-content-sha256"))
            {
                request.Headers.Add("x-amz-content-sha256", ComputeSHA256(request.Content));
            }

            var signingKey = GetSigningKey(credentials, scope);

            var stringToSign = GetStringToSign(scope, request);

            var signature = Signature.ComputeHmacSha256(signingKey, Encoding.UTF8.GetBytes(stringToSign)).ToHexString();

            var signedHeaders = GetSignedHeaders(request);

            // AWS4-HMAC-SHA256 Credential={0},SignedHeaders={0},Signature={0}
            var auth = $"AWS4-HMAC-SHA256 Credential={credentials.AccessKeyId}/{scope},SignedHeaders={signedHeaders},Signature={signature}";

            // AWS4-HMAC-SHA256 Credential=AKIAIOSFODNN7EXAMPLE/20120228/us-east-1/iam/aws4_request,SignedHeaders=content-type;host;x-amz-date,Signature=HexEncode(calculated-signature-from-task-3)

            request.Headers.TryAddWithoutValidation("Authorization", auth);
        }

        public string CanonicizeQueryString(Uri uri)
        {
            if (string.IsNullOrEmpty(uri.Query) || uri.Query == "?") return string.Empty;

            // e.g. ?a=1&

            var sb = new StringBuilder();

            // Sort
            foreach (var pair in ParseQueryString(uri.Query).OrderBy(l => l.Key))
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

        private static IEnumerable<KeyValuePair<string, string>> ParseQueryString(string query)
        {
            if (query[0] == '?')
            {
                query = query.Substring(1);
            }

            foreach (var part in query.Split(Seperators.Ampersand)) // &
            {
                var split = part.Split(Seperators.Equal); // =

                yield return new KeyValuePair<string, string>(
                    key   : Uri.EscapeUriString(split[0]),
                    value : split.Length == 2 ? Uri.EscapeUriString(split[1]) : string.Empty
                );
            }
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

        public SignatureInfo GetInfo(AwsCredentials credentials, CredentialScope scope, HttpRequestMessage request)
        {
            var signingKey = GetSigningKey(credentials, scope);

            var stringToSign = GetStringToSign(scope, request);

            var signature = Signature.ComputeHmacSha256(signingKey, Encoding.UTF8.GetBytes(stringToSign)).ToHexString();

            var signedHeaders = GetSignedHeaders(request);

            var auth = $"AWS4-HMAC-SHA256 Credential={credentials.AccessKeyId}/{scope},SignedHeaders={signedHeaders},Signature={signature}";

            return new SignatureInfo {
                CanonicalizedString = GetCanonicalRequest(request),
                StringToSign = stringToSign,
                Auth = auth
            };
        }

        public static byte[] ComputeSHA256(string text)
        {
            using (var algorithm = SHA256.Create())
            {
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(text));
            }
        }

        public class SignatureInfo
        {
            public string CanonicalizedString { get; set; }

            public string StringToSign { get; set; }

            public string Auth { get; set; }
        }
    }
}