using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;

using Amazon.Helpers;

namespace Amazon.Security
{
    public static class SignerV4
    {
        private const string algorithmName = "AWS4-HMAC-SHA256";
        private const string isoDateTimeFormat = "yyyyMMddTHHmmssZ";  // ISO8601

        public static string GetStringToSign(CredentialScope scope, HttpRequestMessage request)
        {
            return GetStringToSign(scope, request, out _);
        }

        public static string GetStringToSign(CredentialScope scope, HttpRequestMessage request, out List<string> signedHeaders)
        {
            string timestamp = request.Headers.TryGetValues("x-amz-date", out IEnumerable<string> dateHeaderValues)
                ? dateHeaderValues.First()
                : throw new Exception("Missing 'x-amz-date' header");

            return GetStringToSign(
                scope            : scope,
                timestamp        : timestamp,
                canonicalRequest : GetCanonicalRequest(request, out signedHeaders)
            );
        }

        public static string GetStringToSign(in CredentialScope scope, string timestamp, string canonicalRequest)
        {
            string hashedCanonicalRequest = HexString.FromBytes(ComputeSHA256(canonicalRequest));

            using var output = new StringWriter();

            output.Write(algorithmName)           ;  output.Write('\n'); // Algorithm + \n
            output.Write(timestamp)               ;  output.Write('\n'); // Timestamp + \n
            scope.WriteTo(output)                 ;  output.Write('\n'); // Scope     + \n
            output.Write(hashedCanonicalRequest);                        // Hex(SHA256(CanonicalRequest))

            return output.ToString();
        }

        // Timestamp format: ISO8601 Basic format, YYYYMMDD'T'HHMMSS'Z'

        public static string GetCanonicalRequest(HttpRequestMessage request)
        {
            return GetCanonicalRequest(request, out _);
        }
        
        public static string GetCanonicalRequest(HttpRequestMessage request, out List<string> signedHeaderNames)
        {
            using var output = new StringWriter();

            output.Write(request.Method.Method)                               ; output.Write('\n'); // HTTPRequestMethod      + \n
            WriteCanonicalizedUri(output, request.RequestUri!.AbsolutePath)   ; output.Write('\n'); // CanonicalURI           + \n
            WriteCanonicalizedQueryString(output, request.RequestUri)         ; output.Write('\n'); // CanonicalQueryString   + \n
            WriteCanonicalizedHeaders(output, request, out signedHeaderNames) ; output.Write('\n'); // CanonicalHeaders       + \n
            output.Write(string.Empty)                                        ; output.Write('\n'); //                        + \n
            WriteList(output, ';', signedHeaderNames)                         ; output.Write('\n'); // SignedHeaders          + \n
            output.Write(GetPayloadHash(request));                                                  // HexEncode(Hash(Payload))

            return output.ToString();
        }

        private static void WriteList(TextWriter writer, char seperator, List<string> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (i > 0)
                {
                    writer.Write(seperator);
                }

                writer.Write(items[i]);
            }
        }

        public static string CanonicalizeUri(string path)
        {
            if (path.Length == 1 && path[0] == '/')
            {
                return "/";
            }

            using var output = new StringWriter();

            WriteCanonicalizedUri(output, path);

            return output.ToString();
        }

        private static void WriteCanonicalizedUri(StringWriter output, string path)
        {
            if (path.Length == 1 && path[0] == '/')
            {
                output.Write('/');
            }

            var splitter = new Splitter(path.AsSpan(), '/');

            while (splitter.TryGetNext(out var segment))
            {
                if (segment.Length == 0) continue;

                output.Write('/');

                // Do not double escape
                if (segment.IndexOf('%') > -1)
                {
#if NETSTANDARD2_0
                    output.Write(segment.ToString());
#else
                    output.Write(segment);
#endif
                }
                else
                {
                    output.Write(Uri.EscapeDataString(segment.ToString()));
                }
            }
        }

        public static string GetCanonicalRequest(
            HttpMethod method,
            string canonicalURI,
            string canonicalQueryString,
            string canonicalHeaders,
            string signedHeaders,
            string payloadHash)
        {
            var sb = new StringBuilder();

            sb.Append(method.Method)        .Append('\n'); // HTTPRequestMethod           + \n
            sb.Append(canonicalURI)         .Append('\n'); // CanonicalURI                + \n
            sb.Append(canonicalQueryString) .Append('\n'); // CanonicalQueryString        + \n
            sb.Append(canonicalHeaders)     .Append('\n'); // CanonicalHeaders            + \n
            sb.Append(string.Empty)         .Append('\n'); //                             + \n
            sb.Append(signedHeaders)        .Append('\n'); // SignedHeaders               + \n
            sb.Append(payloadHash);                        // HexEncode(Hash(Payload))

            return sb.ToString();
        }

        // HexEncode(Hash(Payload))
        // If the payload is empty, use an empty string
        private static string GetPayloadHash(HttpRequestMessage request)
        {
            // http://docs.aws.amazon.com/AmazonS3/latest/API/sigv4-streaming.html
            // x-amz-content-sha256:e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b785

            // STREAMING-AWS4-HMAC-SHA256-PAYLOAD
            // UNSIGNED-PAYLOAD

            return request.Headers.TryGetValues("x-amz-content-sha256", out var contentSha256Header)
                ? contentSha256Header.First()
                : ComputeSHA256(request.Content);
        }

        private static readonly byte[] aws4_request_bytes = Encoding.ASCII.GetBytes("aws4_request");

        public static byte[] GetSigningKey(string secretAccessKey, in CredentialScope scope)
        {
            static byte[] GetBytes(string text) => Encoding.ASCII.GetBytes(text);
           
            byte[] kSecret    = GetBytes("AWS4" + secretAccessKey);

            byte[] kDate      = HMACSHA256(kSecret,  GetBytes(scope.Date.ToString("yyyyMMdd", CultureInfo.InvariantCulture)));
            byte[] kRegion    = HMACSHA256(kDate,    GetBytes(scope.Region.Name));
            byte[] kService   = HMACSHA256(kRegion,  GetBytes(scope.Service.Name));
            byte[] signingKey = HMACSHA256(kService, aws4_request_bytes);

            return signingKey;
        }

        // http://docs.aws.amazon.com/general/latest/gr/sigv4-add-signature-to-request.html

        public static void Presign(
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
                credential  : credential,
                scope       : scope,
                date        : date,
                expires     : expires,
                method      : request.Method,
                requestUri  : request.RequestUri,
                payloadHash : payloadHash
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
            const string signedHeaders = "host";

            byte[] signingKey = GetSigningKey(credential.SecretAccessKey, scope);

            SortedDictionary<string, string> queryParameters = !string.IsNullOrEmpty(requestUri.Query)
                ? ParseQueryString(requestUri.Query)
                : new SortedDictionary<string, string>();
      
            string timestamp = date.ToString(format: isoDateTimeFormat, CultureInfo.InvariantCulture);
            
            queryParameters[SigningParameterNames.Algorithm]  = algorithmName;
            queryParameters[SigningParameterNames.Credential] = credential.AccessKeyId + "/" + scope;
            queryParameters[SigningParameterNames.Date]       = timestamp;
            queryParameters[SigningParameterNames.Expires]    = expires.TotalSeconds.ToString(CultureInfo.InvariantCulture); // in seconds

            if (credential.SecurityToken != null)
            {
                queryParameters[SigningParameterNames.SecurityToken] = credential.SecurityToken;
            }

            queryParameters[SigningParameterNames.SignedHeaders] = signedHeaders;

            string canonicalHeaders = requestUri.IsDefaultPort
                ? "host:" + requestUri.Host
                : "host:" + requestUri.Host + ":" + requestUri.Port.ToString(CultureInfo.InvariantCulture);

            string canonicalRequest = GetCanonicalRequest(
                method               : method,
                canonicalURI         : CanonicalizeUri(requestUri.AbsolutePath),
                canonicalQueryString : CanonicalizeQueryString(queryParameters),
                canonicalHeaders     : canonicalHeaders,
                signedHeaders        : signedHeaders,
                payloadHash          : payloadHash
            );

            string stringToSign = GetStringToSign(
                scope,
                timestamp,
                canonicalRequest
            );

            Signature signature = Signature.ComputeHmacSha256(
                key  : signingKey,
                data : Encoding.UTF8.GetBytes(stringToSign)
            );

            /*
            queryString = Action=action
            queryString += &X-Amz-Algorithm=algorithm
            queryString += &X-Amz-Credential= urlencode(access_key_ID + '/' + credential_scope)
            queryString += &X-Amz-Date=date
            queryString += &X-Amz-Expires=timeout interval
            queryString += &X-Amz-SignedHeaders=signed_headers
            */

            using var newUrlOuput = new StringWriter();

            newUrlOuput.Write("https://");
            newUrlOuput.Write(requestUri.Host);

            if (!requestUri.IsDefaultPort)
            {
                newUrlOuput.Write(':');
                newUrlOuput.Write(requestUri.Port);
            }

            newUrlOuput.Write(requestUri.AbsolutePath);
            newUrlOuput.Write('?');

            foreach (KeyValuePair<string, string> pair in queryParameters)
            {
                UrlEncoder.Default.Encode(newUrlOuput, pair.Key);
                newUrlOuput.Write('=');
                UrlEncoder.Default.Encode(newUrlOuput, pair.Value);
                newUrlOuput.Write('&');
            }

            newUrlOuput.Write(SigningParameterNames.Signature);
            newUrlOuput.Write('=');
            signature.WriteHexString(newUrlOuput);

            return newUrlOuput.ToString();
        }

        public static void Sign(IAwsCredential credential, CredentialScope scope, HttpRequestMessage request)
        {
            if (credential is null)
            {
                throw new ArgumentNullException(nameof(credential));
            }

            // If we're using S3, ensure the request content has been signed
            if (scope.Service.Equals(AwsService.S3) && !request.Headers.Contains("x-amz-content-sha256"))
            {
                request.Headers.Add("x-amz-content-sha256", ComputeSHA256(request.Content!));
            }

            byte[] signingKey = GetSigningKey(credential.SecretAccessKey, scope);

            string stringToSign = GetStringToSign(scope, request, out var signedHeaderNames);

            Signature signature = Signature.ComputeHmacSha256(signingKey, Encoding.UTF8.GetBytes(stringToSign));

            using var authWriter = new StringWriter();

            // AWS4-HMAC-SHA256 Credential={0},SignedHeaders={0},Signature={0}
            // var auth = $"AWS4-HMAC-SHA256 Credential={credential.AccessKeyId}/{scope},SignedHeaders={signedHeaders},Signature={signature}";

            authWriter.Write("AWS4-HMAC-SHA256 Credential=");
            authWriter.Write(credential.AccessKeyId);
            authWriter.Write('/');
            scope.WriteTo(authWriter);
            authWriter.Write(",SignedHeaders=");
            WriteList(authWriter, ';', signedHeaderNames);
            authWriter.Write(",Signature=");
            signature.WriteHexString(authWriter);

            request.Headers.TryAddWithoutValidation("Authorization", authWriter.ToString());
        }

        public static string CanonicalizeQueryString(Uri uri)
        {
            if (string.IsNullOrEmpty(uri.Query) || uri.Query.Length == 1 && uri.Query[0] == '?')
            {
                return string.Empty;
            }

            return CanonicalizeQueryString(ParseQueryString(uri.Query));
        }

        private static string CanonicalizeQueryString(SortedDictionary<string, string> sortedValues)
        {
            if (sortedValues.Count == 0)
            {
                return string.Empty;
            }

            using var output = new StringWriter();

            WriteCanonicalQueryString(output, sortedValues);

            return output.ToString();
        }

        private static void WriteCanonicalizedQueryString(StringWriter output, Uri uri)
        {
            if (string.IsNullOrEmpty(uri.Query) || uri.Query.Length == 1 && uri.Query[0] == '?')
            {
                return;
            }

            WriteCanonicalQueryString(output, ParseQueryString(uri.Query));
        }

        private static void WriteCanonicalQueryString(TextWriter output, SortedDictionary<string, string> sortedValues)
        {
            int i = 0;

            foreach (var pair in sortedValues)
            {
                if (i > 0)
                {
                    output.Write('&');
                }

                UrlEncoder.Default.Encode(output, pair.Key);
                output.Write("=");
                UrlEncoder.Default.Encode(output, pair.Value);

                i++;
            }
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

            while (splitter.TryGetNext(out ReadOnlySpan<char> part))
            {
                if (part.Length == 0) continue;

                int equalIndex = part.IndexOf('=');

                string lhs  = equalIndex > -1 ? part.Slice(0, equalIndex).ToString() : part.ToString();
                string? rhs = equalIndex > -1 ? part.Slice(equalIndex + 1).ToString() : null;

                dictionary[WebUtility.UrlDecode(lhs)] = rhs != null
                    ? WebUtility.UrlDecode(rhs)
                    : string.Empty;
            }

            return dictionary;
        }


        public static string CanonicalizeHeaders(HttpRequestMessage request, out List<string> signedHeaderNames)
        {
            using var writer = new StringWriter();

            WriteCanonicalizedHeaders(writer, request, out signedHeaderNames);

            return writer.ToString();
        }

        public static void WriteCanonicalizedHeaders(TextWriter writer, HttpRequestMessage request, out List<string> signedHeaderNames)
        {
            signedHeaderNames = new List<string>(8);

            if (request.Content != null && request.Content.Headers.TryGetValues("Content-MD5", out var md5Header))
            {
                signedHeaderNames.Add("content-md5");

                writer.Write("content-md5:");
                writer.Write(md5Header.First());
                writer.Write('\n');
            }

            if (!request.Headers.Contains("x-amz-date") && request.Headers.TryGetValues("Date", out var dateHeader))
            {
                signedHeaderNames.Add("date");

                writer.Write("date:");
                writer.Write(dateHeader.First());
                writer.Write('\n');
            }

            signedHeaderNames.Add("host");

            writer.Write("host:");
            writer.Write(request.Headers.Host);

            foreach (var header in request.Headers
                .Where(item => item.Key.StartsWith("x-amz-", StringComparison.OrdinalIgnoreCase))
                .OrderBy(item => item.Key.ToLowerInvariant()))
            {
                var headerName = header.Key.ToLowerInvariant();

                writer.Write('\n');

                writer.Write(headerName);
                writer.Write(':');
                writer.Write(header.Value.First());

                signedHeaderNames.Add(headerName);
            }
        }

        // Convert all header names to lowercase
        // Sort them by character code
        // Use a semicolon to separate the header names

        // The host header must be included as a signed header.

        #region SHA Helpers

        private const string emptySha256 = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";

        public static byte[] ComputeSHA256(string text)
        {
            return ComputeSHA256(Encoding.UTF8.GetBytes(text));
        }

        public static byte[] ComputeSHA256(byte[] data)
        {
#if NET5_0
            return SHA256.HashData(data);
#else
            using SHA256 algorithm = SHA256.Create();

            return algorithm.ComputeHash(data);
#endif
        }

        public static string ComputeSHA256(HttpContent content)
        {
            return content?.ReadAsByteArrayAsync().Result is byte[] { Length: > 0 } data
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

// Signed Header Notes ---

// - Convert all header names to lowercase
// - Sort them by character code
// - Use a semicolon to separate the header names