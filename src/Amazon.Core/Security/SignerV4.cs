using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;

using Amazon.Helpers;

namespace Amazon.Security
{
    public sealed class SignerV4
    {
        private const string algorithmName = "AWS4-HMAC-SHA256";
        private const string isoDateTimeFormat = "yyyyMMddTHHmmssZ";  // ISO8601

        public static readonly SignerV4 Default = new SignerV4();

        public string GetStringToSign(CredentialScope scope, HttpRequestMessage request)
        {
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

        public static string GetStringToSign(in CredentialScope scope, string timestamp, string canonicalRequest)
        {
            string hashedCanonicalRequest = HexString.FromBytes(ComputeSHA256(canonicalRequest));

            var sb = StringBuilderCache.Aquire();

            sb.Append(algorithmName).Append('\n');    // Algorithm + \n
            sb.Append(timestamp).Append('\n');        // Timestamp + \n
            sb.Append(scope.ToString()).Append('\n'); // Scope     + \n
            sb.Append(hashedCanonicalRequest);        // Hex(SHA256(CanonicalRequest))

            return StringBuilderCache.ExtractAndRelease(sb);
        }

        // Timestamp format: ISO8601 Basic format, YYYYMMDD'T'HHMMSS'Z'

        public static string GetCanonicalRequest(HttpRequestMessage request)
        {
            var sb = StringBuilderCache.Aquire();

            sb.Append(request.Method.Method)                            .Append('\n'); // HTTPRequestMethod      + \n
            sb.Append(CanonicalizeUri(request.RequestUri.AbsolutePath)) .Append('\n'); // CanonicalURI           + \n
            sb.Append(CanonicizeQueryString(request.RequestUri))        .Append('\n'); // CanonicalQueryString   + \n
            sb.Append(CanonicalizeHeaders(request))                     .Append('\n'); // CanonicalHeaders       + \n
            sb.Append(string.Empty)                                     .Append('\n'); //                        + \n
            sb.Append(GetSignedHeaders(request))                        .Append('\n'); // SignedHeaders          + \n
            sb.Append(GetPayloadHash(request));                                        // HexEncode(Hash(Payload))
        
            return StringBuilderCache.ExtractAndRelease(sb);
        }
        
        public static string CanonicalizeUri(string path)
        {
            if (path.Length == 1 && path[0] == '/') return "/";

            var splitter = new Splitter(path.AsSpan(), '/');

            var sb = StringBuilderCache.Aquire();

            while (splitter.TryGetNext(out var segment))
            {
                if (segment.Length == 0) continue;

                sb.Append('/');

                // Do not double escape
                if (segment.IndexOf('%') > -1)
                {
#if NETSTANDARD2_0
                    sb.Append(segment.ToString());
#else
                    sb.Append(segment);
#endif
                }
                else
                {
                    sb.Append(Uri.EscapeDataString(segment.ToString()));
                }
            }

            return StringBuilderCache.ExtractAndRelease(sb);
        }

        public static string GetCanonicalRequest(
            HttpMethod method,
            string canonicalURI,
            string canonicalQueryString,
            string canonicalHeaders,
            string signedHeaders,
            string payloadHash)
        {
            var sb = StringBuilderCache.Aquire();

            sb.Append(method.ToString())    .Append('\n'); // HTTPRequestMethod           + \n
            sb.Append(canonicalURI)         .Append('\n'); // CanonicalURI                + \n
            sb.Append(canonicalQueryString) .Append('\n'); // CanonicalQueryString        + \n
            sb.Append(canonicalHeaders)     .Append('\n'); // CanonicalHeaders            + \n
            sb.Append(string.Empty)         .Append('\n'); //                             + \n
            sb.Append(signedHeaders)        .Append('\n'); // SignedHeaders               + \n
            sb.Append(payloadHash);                        // HexEncode(Hash(Payload))
         
            return StringBuilderCache.ExtractAndRelease(sb);
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

        private static readonly byte[] aws4_request_bytes = Encoding.ASCII.GetBytes("aws4_request");

        public static byte[] GetSigningKey(string secretAccessKey, in CredentialScope scope)
        {
            static byte[] GetBytes(string text)
            {
                return Encoding.ASCII.GetBytes(text);
            }

            byte[] kSecret    = GetBytes("AWS4" + secretAccessKey);

            byte[] kDate      = HMACSHA256(kSecret,  GetBytes(scope.Date.ToString("yyyyMMdd", CultureInfo.InvariantCulture)));
            byte[] kRegion    = HMACSHA256(kDate,    GetBytes(scope.Region.Name));
            byte[] kService   = HMACSHA256(kRegion,  GetBytes(scope.Service.Name));
            byte[] signingKey = HMACSHA256(kService, aws4_request_bytes);

            return signingKey;
        }

        // http://docs.aws.amazon.com/general/latest/gr/sigv4-add-signature-to-request.html

        public void Presign(
            IAwsCredential credential,
            CredentialScope scope,
            DateTime date,
            TimeSpan expires,
            HttpRequestMessage request,
            string payloadHash = emptySha256)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            string presignedUrl = GetPresignedUrl(
                credential            : credential,
                scope                 : scope,
                date                  : date,
                expires               : expires,
                method                : request.Method,
                requestUri            : request.RequestUri,
                payloadHash           : payloadHash
            );

            request.RequestUri = new Uri(presignedUrl);
        }

        public static string GetPresignedUrl(
            IAwsCredential credential,
            CredentialScope scope,
            DateTime date,
            TimeSpan expires,
            HttpMethod method,
            Uri requestUri,
            string payloadHash = emptySha256)
        {
            byte[] signingKey = GetSigningKey(credential.SecretAccessKey, scope);

            SortedDictionary<string, string> queryParameters = !string.IsNullOrEmpty(requestUri.Query)
                ? ParseQueryString(requestUri.Query)
                : new SortedDictionary<string, string>();
      
            string timestamp = date.ToString(format: isoDateTimeFormat, CultureInfo.InvariantCulture);
            string signedHeaders = "host";

            queryParameters[SigningParameterNames.Algorithm] = algorithmName;
            queryParameters[SigningParameterNames.Credential] = credential.AccessKeyId + "/" + scope;

            if (credential.SecurityToken != null)
            {
                queryParameters[SigningParameterNames.SecurityToken] = credential.SecurityToken;
            }

            queryParameters[SigningParameterNames.Date]          = timestamp;
            queryParameters[SigningParameterNames.Expires]       = expires.TotalSeconds.ToString(CultureInfo.InvariantCulture); // in seconds
            queryParameters[SigningParameterNames.SignedHeaders] = signedHeaders;

            string canonicalHeaders = requestUri.IsDefaultPort
                ? "host:" + requestUri.Host
                : "host:" + requestUri.Host + ":" + requestUri.Port.ToString(CultureInfo.InvariantCulture);

            string canonicalRequest = GetCanonicalRequest(
                method               : method,
                canonicalURI         : CanonicalizeUri(requestUri.AbsolutePath),
                canonicalQueryString : CanonicizeQueryString(queryParameters),
                canonicalHeaders     : canonicalHeaders,
                signedHeaders        : signedHeaders,
                payloadHash          : payloadHash
            );

            string stringToSign = GetStringToSign(
                scope,
                timestamp,
                canonicalRequest
            );

            string signature = Signature.ComputeHmacSha256(
                key: signingKey,
                data: Encoding.UTF8.GetBytes(stringToSign)
            ).ToHexString();

            /*
            queryString = Action=action
            queryString += &X-Amz-Algorithm=algorithm
            queryString += &X-Amz-Credential= urlencode(access_key_ID + '/' + credential_scope)
            queryString += &X-Amz-Date=date
            queryString += &X-Amz-Expires=timeout interval
            queryString += &X-Amz-SignedHeaders=signed_headers
            */

            var newUrl = StringBuilderCache.Aquire();

            newUrl.Append("https://").Append(requestUri.Host);

            if (!requestUri.IsDefaultPort)
            {
                newUrl.Append(':');
                newUrl.Append(requestUri.Port);
            }

            newUrl.Append(requestUri.AbsolutePath);
            newUrl.Append('?');

            foreach (KeyValuePair<string, string> pair in queryParameters)
            {
                newUrl.Append(UrlEncoder.Default.Encode(pair.Key));
                newUrl.Append('=');
                newUrl.Append(UrlEncoder.Default.Encode(pair.Value));
                newUrl.Append('&');
            }

            newUrl.Append(SigningParameterNames.Signature).Append('=').Append(signature);

            return StringBuilderCache.ExtractAndRelease(newUrl);
        }

        public void Sign(IAwsCredential credential, CredentialScope scope, HttpRequestMessage request)
        {
            if (credential is null)
                throw new ArgumentNullException(nameof(credential));

            // If we're using S3, ensure the request content has been signed
            if (scope.Service.Equals(AwsService.S3) && !request.Headers.Contains("x-amz-content-sha256"))
            {
                request.Headers.Add("x-amz-content-sha256", ComputeSHA256(request.Content));
            }

            byte[] signingKey = GetSigningKey(credential.SecretAccessKey, scope);

            string stringToSign = GetStringToSign(scope, request);

            var signature = Signature.ComputeHmacSha256(signingKey, Encoding.UTF8.GetBytes(stringToSign)).ToHexString();

            string signedHeaders = GetSignedHeaders(request);

            // AWS4-HMAC-SHA256 Credential={0},SignedHeaders={0},Signature={0}
            var auth = $"AWS4-HMAC-SHA256 Credential={credential.AccessKeyId}/{scope},SignedHeaders={signedHeaders},Signature={signature}";

            request.Headers.TryAddWithoutValidation("Authorization", auth);
        }

        // e.g. ?a=1&

        public static string CanonicizeQueryString(Uri uri)
        {
            if (string.IsNullOrEmpty(uri.Query) || uri.Query.Length == 1 && uri.Query[0] == '?')
            {
                return string.Empty;
            }

            return CanonicizeQueryString(ParseQueryString(uri.Query));
        }

        public static string CanonicizeQueryString(SortedDictionary<string, string> sortedValues)
        {
            if (sortedValues.Count == 0)
            {
                return string.Empty;
            }

            var sb = StringBuilderCache.Aquire();

            foreach (var pair in sortedValues)
            {
                if (sb.Length > 0)
                {
                    sb.Append('&');
                }

                sb.Append(UrlEncoder.Default.Encode(pair.Key));
                sb.Append('=');
                sb.Append(UrlEncoder.Default.Encode(pair.Value));
            }

            return StringBuilderCache.ExtractAndRelease(sb);
        }

        private static SortedDictionary<string, string> ParseQueryString(string query)
        {
            return ParseQueryString(query.AsSpan());
        }

        private static SortedDictionary<string, string> ParseQueryString(ReadOnlySpan<char> query)
        {
            if (query.Length == 0)
            {
                return new SortedDictionary<string, string>();
            }

            var dictionary = new SortedDictionary<string, string>();

            if (query[0] == '?')
            {
                query = query.Slice(1);
            }

            var splitter = new Splitter(query, '&');

            while (splitter.TryGetNext(out var part))
            {
                int equalIndex = part.IndexOf('=');

                string lhs  = equalIndex > -1 ? part.Slice(0, equalIndex).ToString() : part.ToString();
                string? rhs = equalIndex > -1 ? part.Slice(equalIndex + 1).ToString() : null;

                dictionary[WebUtility.UrlDecode(lhs)] = rhs != null
                    ? WebUtility.UrlDecode(rhs)
                    : string.Empty;
            }

            return dictionary;
        }

        public static string GetSignedHeaders(HttpRequestMessage request)
        {
            // Convert all header names to lowercase
            // Sort them by character code
            // Use a semicolon to separate the header names

            // The host header must be included as a signed header.

            var sb = StringBuilderCache.Aquire();

            if (request.Content?.Headers.Contains("Content-MD5") == true)
            {
                sb.Append("content-md5;");
            }

            if (!request.Headers.Contains("x-amz-date") && request.Headers.Contains("Date"))
            {
                sb.Append("date;");
            }

            sb.Append("host");

            foreach (var header in request.Headers
                .Where(item => item.Key.StartsWith("x-amz-", StringComparison.OrdinalIgnoreCase))
                .Select(item => item.Key.ToLowerInvariant())
                .OrderBy(k => k))
            {
                sb.Append(';');
                sb.Append(header);
            }

            return StringBuilderCache.ExtractAndRelease(sb);
        }

        public static string CanonicalizeHeaders(HttpRequestMessage request)
        {
            var sb = StringBuilderCache.Aquire();

            if (request.Content?.Headers.Contains("Content-MD5") == true)
            {
                sb.Append("content-md5:");
                sb.Append(request.Content.Headers.GetValues("Content-MD5").FirstOrDefault());
                sb.Append('\n');
            }

            if (!request.Headers.Contains("x-amz-date") && request.Headers.Contains("Date"))
            {
                sb.Append("date:");
                sb.Append(request.Headers.GetValues("Date").First());
                sb.Append('\n');
            }

            sb.Append("host:").Append(request.Headers.Host);

            foreach (var header in request.Headers
                .Where(item => item.Key.StartsWith("x-amz-", StringComparison.OrdinalIgnoreCase))
                .OrderBy(item => item.Key.ToLowerInvariant()))
            {
                sb.Append('\n');

                sb.Append(header.Key.ToLowerInvariant());
                sb.Append(':');
                sb.AppendJoin(';', header.Value);
            }

            return StringBuilderCache.ExtractAndRelease(sb);
        }

        #region SHA Helpers

        private const string emptySha256 = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";

        public static byte[] ComputeSHA256(string text)
        {
            using SHA256 algorithm = SHA256.Create();

            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(text));
        }

        public static byte[] ComputeSHA256(byte[] data)
        {
            using SHA256 algorithm = SHA256.Create();

            return algorithm.ComputeHash(data);
        }

        public static string ComputeSHA256(HttpContent content)
        {
            return content?.ReadAsByteArrayAsync().Result is byte[] data && data.Length > 0
                ? HexString.FromBytes(ComputeSHA256(data))
                : emptySha256;
        }

        private static byte[] HMACSHA256(byte[] key, byte[] data)
        {
            using var hmac = new HMACSHA256(key);

            return hmac.ComputeHash(data);
        }

        #endregion
    }
}