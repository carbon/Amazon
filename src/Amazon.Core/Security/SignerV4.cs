using System;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace Amazon.Security
{
    using Helpers;

    public class SignerV4 : SignerV3
    {
        public string GenerateStringToSign(CredentialScope scope, HttpRequestMessage request)
        {
            var dateHeader = request.Headers.GetValues("x-amz-date").First();
            var canonicalRequest = HexString.FromBytes(ComputeHash(CanonicalizeRequest(request)));

            var sb = new StringBuilder()
                .Append("AWS4-HMAC-SHA256")     .Append("\n") // Algorithm
                .Append(dateHeader)             .Append("\n") // RequestDate (ISO8601 Basic format, YYYYMMDD'T'HHMMSS'Z')
                .Append(scope.ToString())       .Append("\n") // CredentialScope
                .Append(canonicalRequest);                    // HexEncode(Hash(CanonicalRequest))

            return sb.ToString();
        }

        public string CanonicalizeRequest(HttpRequestMessage request)
        {
            var canonicalQueryString = CanonicizeQueryString(request.RequestUri);

            return new StringBuilder()
                .Append(request.Method)                     .Append("\n") // HTTPRequestMethod
                .Append(request.RequestUri.AbsolutePath)    .Append("\n") // CanonicalURI
                .Append(canonicalQueryString)               .Append("\n") // CanonicalQueryString
                .Append(CanonicalizeHeaders(request))       .Append("\n") // CanonicalHeaders
                .Append("\n")
                .Append(GetSignedHeaders(request))          .Append("\n") // SignedHeaders
                .Append(HashPayload(request))                             // HexEncode(Hash(Payload))
                .ToString();
        }

        public string HashPayload(HttpRequestMessage request)
        {
            // HexEncode(Hash(Payload))
            // If the payload is empty, use an empty string
            return HexString.FromBytes(ComputeHash(request.Content?.ReadAsStringAsync().Result ?? string.Empty));
        }

        private byte[] HmacSHA256(byte[] key, String data)
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

            var kDate = HmacSHA256(kSecret, scope.Date.ToString("yyyyMMdd"));
            var kRegion = HmacSHA256(kDate, scope.Region.Name);
            var kService = HmacSHA256(kRegion, scope.Service.Name);
            var signingKey = HmacSHA256(kService, "aws4_request");

            return signingKey;
        }

        public void Sign(IAwsCredentials credentials, CredentialScope scope, HttpRequestMessage httpRequest)
        {
            #region Preconditions

            if (credentials == null)    throw new ArgumentNullException(nameof(credentials));
            if (scope == null)          throw new ArgumentNullException(nameof(scope));
            if (httpRequest == null)    throw new ArgumentNullException(nameof(httpRequest));

            #endregion

            var signingKey = GetSigningKey(credentials, scope);

            var stringToSign = GenerateStringToSign(scope, httpRequest);

            var signature = Signature.ComputeHmacSha256(signingKey, Encoding.UTF8.GetBytes(stringToSign)).ToHexString();

            var signedHeaders = GetSignedHeaders(httpRequest);

            // AWS4-HMAC-SHA256 Credential={0},SignedHeaders={0},Signature={0}
            var auth = $"AWS4-HMAC-SHA256 Credential={credentials.AccessKeyId}/{scope},SignedHeaders={signedHeaders},Signature={signature}";

            // AWS4-HMAC-SHA256 Credential=AKIAIOSFODNN7EXAMPLE/20120228/us-east-1/iam/aws4_request,SignedHeaders=content-type;host;x-amz-date,Signature=HexEncode(calculated-signature-from-task-3)

            httpRequest.Headers.TryAddWithoutValidation("Authorization", auth);
        }

        public SignatureInfo GetInfo(AwsCredentials credentials, CredentialScope scope, HttpRequestMessage request)
        {
            var signingKey = GetSigningKey(credentials, scope);

            var stringToSign = GenerateStringToSign(scope, request);

            var signature = Signature.ComputeHmacSha256(signingKey, Encoding.UTF8.GetBytes(stringToSign)).ToHexString();

            var signedHeaders = GetSignedHeaders(request);

            var auth = $"AWS4-HMAC-SHA256 Credential={credentials.AccessKeyId}/{scope},SignedHeaders={signedHeaders},Signature={signature}";

            return new SignatureInfo {
                CanonicalizedString = CanonicalizeRequest(request),
                StringToSign = stringToSign,
                Auth = auth
            };
        }

        public class SignatureInfo
        {
            public string CanonicalizedString { get; set; }

            public string StringToSign { get; set; }

            public string Auth { get; set; }
        }
    }
}