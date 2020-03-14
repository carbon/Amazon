using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Carbon.Storage;

namespace Amazon.S3
{
    public sealed class S3Client : AwsClient
    {
        public const string Namespace = "http://s3.amazonaws.com/doc/2006-03-01/";

        public S3Client(AwsRegion region, IAwsCredential credential)
            : this(region, host: $"s3.dualstack.{region.Name}.amazonaws.com", credential: credential) { }
        
        public S3Client(AwsRegion region, string host, IAwsCredential credential)
            : base(AwsService.S3, region, credential)
        {
            Host = host ?? throw new ArgumentNullException(nameof(host));
        }

        public void SetTimeout(TimeSpan timeout)
        {
            httpClient.Timeout = timeout;
        }

        public string Host { get; }
        
        // TODO: CreateBucketAsync

        public async Task<ListBucketResult> ListBucketAsync(string bucketName, ListBucketOptions options)
        {
            var request = new ListBucketRequest(Host, bucketName, options);

            var responseText = await SendAsync(request).ConfigureAwait(false);          

            return ListBucketResult.ParseXml(responseText);
        }

        #region Multipart Uploads

        public async Task AbortMultipartUploadAsync(AbortMultipartUploadRequest request)
        {
            await SendAsync(request).ConfigureAwait(false);
        }

        public async Task<InitiateMultipartUploadResult> InitiateMultipartUploadAsync(InitiateMultipartUploadRequest request)
        {
            string responseText = await SendAsync(request).ConfigureAwait(false);

            return InitiateMultipartUploadResult.ParseXml(responseText);
        }

        public async Task<UploadPartResult> UploadPartAsync(UploadPartRequest request)
        {
            using HttpResponseMessage response = await SendAsync2(request, request.CompletionOption).ConfigureAwait(false);

            return new UploadPartResult(
                uploadId   : request.UploadId,
                partNumber : request.PartNumber,
                eTag       : response.Headers.ETag.Tag
            );
        }

        public async Task<CompleteMultipartUploadResult> CompleteMultipartUploadAsync(CompleteMultipartUploadRequest request)
        {
            var responseText = await SendAsync(request).ConfigureAwait(false);

            return ResponseHelper<CompleteMultipartUploadResult>.ParseXml(responseText);
        }

        #endregion

        public async Task<PutObjectResult> PutObjectAsync(PutObjectRequest request)
        {
            using HttpResponseMessage response = await SendAsync2(request, request.CompletionOption).ConfigureAwait(false);

            string? versionId = null;

            if (response.Headers.TryGetValues("x-amz-version-id", out var xVersionId))
            {
                versionId = xVersionId.ToString();
            }

            return new PutObjectResult(
                eTag      : response.Headers.ETag.Tag, 
                versionId : versionId
            );
        }

        public async Task<CopyObjectResult> CopyObjectAsync(CopyObjectRequest request)
        {
            string responseText = await SendAsync(request).ConfigureAwait(false);

            return CopyObjectResult.ParseXml(responseText);
        }

        public async Task DeleteObjectAsync(DeleteObjectRequest request)
        {
            using HttpResponseMessage response = await SendAsync2(request, request.CompletionOption).ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                throw new S3Exception("Expected 204", response.StatusCode);
            }
        }

        public async Task<DeleteResult> DeleteObjectsAsync(BatchDeleteRequest request)
        {
            var responseText = await SendAsync(request).ConfigureAwait(false);
            
            return DeleteResult.Parse(responseText);
        }

        public async Task<RestoreObjectResult> RestoreObjectAsync(RestoreObjectRequest request)
        {
            using HttpResponseMessage response = await SendAsync2(request, request.CompletionOption).ConfigureAwait(false);

            return new RestoreObjectResult(response.StatusCode);
        }

        public async Task<S3Object> GetObjectAsync(GetObjectRequest request)
        {
            var response = await SendAsync2(request, request.CompletionOption).ConfigureAwait(false);

            return new S3Object(request.ObjectName!, response);
        }

        public async Task<S3ObjectInfo> GetObjectHeadAsync(ObjectHeadRequest request)
        {
            using var response = await SendAsync2(request, request.CompletionOption).ConfigureAwait(false);

            return S3ObjectInfo.FromResponse(request.BucketName, request.ObjectName!, response);
        }

        private async Task<HttpResponseMessage> SendAsync2(HttpRequestMessage request, HttpCompletionOption completionOption)
        {
            await SignAsync(request).ConfigureAwait(false);

            var response = await httpClient.SendAsync(request, completionOption).ConfigureAwait(false);
            
            if (response.StatusCode == HttpStatusCode.NotModified)
            {
                return response;
            }

            if (!response.IsSuccessStatusCode)
            {
                using (response)
                {
                    throw await GetExceptionAsync(response).ConfigureAwait(false);
                }
            }

            return response;
        }

        public string GetPresignedUrl(GetPresignedUrlRequest request)
        {
            return S3Helper.GetPresignedUrl(request, credential);
        }

        #region Helpers

        protected override async Task<Exception> GetExceptionAsync(HttpResponseMessage response)
        {
            string responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                string key = response.RequestMessage.RequestUri.AbsolutePath;

                if (key.Length > 0 && key[0] == '/')
                {
                    key = key.Substring(1);
                }

                throw StorageException.NotFound(key);
            }

            if (responseText.Contains("<Error>"))
            {
                throw new S3Exception(
                    error      : S3Error.ParseXml(responseText),
                    statusCode : response.StatusCode
                );
            }

            throw new S3Exception("Unexpected S3 error. " + response.StatusCode + ":" + responseText, response.StatusCode);
        }

        #endregion
    }
}