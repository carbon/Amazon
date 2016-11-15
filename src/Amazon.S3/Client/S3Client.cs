using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using Carbon.Storage;

namespace Amazon.S3
{
    public class S3Client : IDisposable
    {
        public static readonly XNamespace NS = "http://s3.amazonaws.com/doc/2006-03-01/";

        private readonly AwsRegion region;
        private readonly AwsCredentials credentials;

        private readonly HttpClient httpClient = new HttpClient();

        public S3Client(AwsRegion region, AwsCredentials credentials)
        {
            #region Preconditions

            if (credentials == null) throw new ArgumentNullException("credentials");

            #endregion

            this.region = region;
            this.credentials = credentials;

            #if dotnet451
            ServicePointManager.DefaultConnectionLimit = 5000;
            #endif
        }

        public AwsRegion Region => region;

        public void SetTimeout(TimeSpan timeout)
        {
            httpClient.Timeout = timeout;
        }

        public Task<HttpResponseMessage> CreateBucket(string bucketName)
            => Send(new AddBucketRequest(region, bucketName));

        public async Task<ListBucketResult> ListBucket(string bucketName, ListBucketOptions options)
        {
            var request = new ListBucketRequest(region, bucketName, options);

            using (var response = await Send(request).ConfigureAwait(false))
            {
                var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return ListBucketResult.ParseXml(responseText);
            }
        }

        #region Multipart Uploads

        public async Task<InitiateMultipartUploadResult> InitiateMultipartUpload(InitiateMultipartUploadRequest request)
        {
            using (var response = await Send(request).ConfigureAwait(false))
            {
                var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return InitiateMultipartUploadResult.ParseXml(responseText);
            }
        }

        public async Task<UploadPartResult> UploadPart(UploadPartRequest request)
        {
            using (var response = await Send(request).ConfigureAwait(false))
            {
                return new UploadPartResult(
                    partNumber: request.PartNumber,
                    uploadId: request.UploadId,
                    eTag: response.Headers.ETag.Tag
                );
            }
        }

        public async Task<CompleteMultipartUploadResult> CompleteMultipartUpload(CompleteMultipartUploadRequest request)
        {
            using (var response = await Send(request).ConfigureAwait(false))
            {
                var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return CompleteMultipartUploadResult.ParseXml(responseText);
            }
        }

        #endregion

        public async Task<PutObjectResult> PutObject(PutObjectRequest request)
        {
            using (var response = await Send(request).ConfigureAwait(false))
            {
                return new PutObjectResult(response);
            }
        }

        public async Task<CopyObjectResult> CopyObject(CopyObjectRequest request)
        {
            using (var response = await Send(request).ConfigureAwait(false))
            {
                var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return CopyObjectResult.ParseXml(responseText);
            }
        }

        public async Task DeleteObject(DeleteObjectRequest request)
        {
            using (var response = await Send(request).ConfigureAwait(false))
            {
                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    throw new Exception("Expected 204");
                }
            }
        }

        public async Task<DeleteResult> DeleteObjects(BatchDeleteRequest request)
        {
            using (var response = await Send(request).ConfigureAwait(false))
            {
                var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return DeleteResult.Parse(responseText);
            }
        }

        public async Task<RestoreObjectResult> RestoreObject(RestoreObjectRequest request)
        {
            using (var response = await Send(request).ConfigureAwait(false))
            {
                return new RestoreObjectResult(response);
            }
        }

        public async Task<S3Object> GetObject(GetObjectRequest request)
        {
            var response = await Send(request).ConfigureAwait(false);

            return new S3Object(request.Key, response);
        }

        public async Task<S3ObjectInfo> GetObjectHead(ObjectHeadRequest request)
        {
            using (var response = await Send(request).ConfigureAwait(false))
            {
                return new S3ObjectInfo(
                    bucketName : request.BucketName,
                    name       : request.Key,
                    response   : response
                );
            }
        }

        public string GetSignedUrl(GetUrlRequest request)
        {
            // You can specify any future expiration time in epoch or UNIX time (number of seconds since January 1, 1970).

            long expires = S3Helper.GetSecondsSince1970() + (long)request.ExpiresIn.TotalSeconds;

            string stringToSign = S3Helper.ConstructStringToSign(
                httpVerb: HttpMethod.Get,
                contentType: "",
                bucketName: request.BucketName,
                key: request.Key,
                headers: new Dictionary<string, string>(),
                query: "",
                expiresOrDate: expires.ToString()
            );

            var signature = S3Helper.ComputeSignature(credentials.SecretAccessKey, stringToSign);

            var urlBuilder = new StringBuilder()
                .Append("https://")
                .Append(request.BucketName)
                .Append(".s3.amazonaws.com/")
                .Append(request.Key)
                .Append("?AWSAccessKeyId=")
                .Append(credentials.AccessKeyId.UrlEncodeX2())
                .Append("&Expires=")
                .Append(expires)
                .Append("&Signature=")
                .Append(signature.UrlEncodeX2());

            return urlBuilder.ToString();
        }

        #region Helpers

        private async Task<HttpResponseMessage> Send(S3Request request)
        {
            ConfigureWebRequest(request);

            var response = await httpClient.SendAsync(request, request.CompletionOption).ConfigureAwait(false);

            await ThrowIfErrors(request, response).ConfigureAwait(false);

            return response;
        }

        private async Task ThrowIfErrors(S3Request request, HttpResponseMessage response)
        {
            #region Preconditions

            if (response == null) throw new ArgumentNullException(nameof(response));

            #endregion

            if (!response.IsSuccessStatusCode)
            {
                using (response)
                {
                    var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        // Consider limiting to KeyNotFound?

                        throw StorageException.NotFound(request.Key);
                    }

                    if (responseText.Contains("<Error>"))
                    {
                        var error = S3Error.ParseXml(responseText);

                        throw new S3Exception(error, response.StatusCode, responseText);
                    }

                    throw new S3Exception("Unexpected S3 error. " + response.StatusCode + ":" + responseText, response.StatusCode);
                }
            }
        }

        private void ConfigureWebRequest(S3Request request)
        {
            #region Preconditions

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            #endregion

            request.Headers.Add("User-Agent", "Carbon/1.4");

            request.Headers.Date = DateTimeOffset.UtcNow;

            SignRequest(request, request);
        }

        private void SignRequest(HttpRequestMessage httpRequest, S3Request s3request)
        {
            // TODO: Use V4

            string contentType = null;

            if (s3request.Content != null)
            {
                contentType = string.Join(";", httpRequest.Content.Headers.GetValues("Content-Type"));
            }

            var headers = GetHeaders(s3request);

            var stringToSign = S3Helper.ConstructStringToSign(
                httpVerb: httpRequest.Method,
                contentType: contentType,
                bucketName: s3request.BucketName,
                key: s3request.Key,
                headers: headers,
                query: s3request.RequestUri.Query,
                expiresOrDate: headers["Date"]
            );

            string signature = S3Helper.ComputeSignature(credentials.SecretAccessKey, stringToSign);

            // Authorization: AWS [AWSAccessKeyId]:[Signature]
            httpRequest.Headers.Add("Authorization", $"AWS {credentials.AccessKeyId}:{signature}");
        }

        /*
        private static readonly SignerV4 signer = new SignerV4();

        private void Sign4(HttpRequestMessage httpRequest)
        {
           

            var date = DateTimeOffset.UtcNow;

            httpRequest.Headers.UserAgent.ParseAdd("Carbon/1.3");
            httpRequest.Headers.Host = httpRequest.RequestUri.Host;
            httpRequest.Headers.Date = date;

            if (credentials.SecurityToken != null)
            {
                httpRequest.Headers.Add("X-Amz-Security-Token", credentials.SecurityToken);
            }

            httpRequest.Headers.Add("x-amz-date", date.UtcDateTime.ToString("yyyyMMddTHHmmssZ"));

            var scope = GetCredentialScope(httpRequest);

            signer.Sign(credentials, scope, httpRequest);
        }

        private CredentialScope GetCredentialScope(HttpRequestMessage httpRequest)
           => new CredentialScope(httpRequest.Headers.Date.Value.UtcDateTime, region, service);
        */

        public Dictionary<string, string> GetHeaders(HttpRequestMessage request)
        {
            var headers = new Dictionary<string, string>();

            foreach (var header in request.Headers)
            {
                headers.Add(header.Key, string.Join("; ", header.Value));
            }

            if (request.Content != null)
            {
                foreach (var header in request.Content.Headers)
                {
                    headers.Add(header.Key, string.Join("; ", header.Value));
                }
            }

            return headers;
        }

        #endregion

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}