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
    public class S3Client : AwsClient
    {
        public static readonly XNamespace NS = "http://s3.amazonaws.com/doc/2006-03-01/";

        public S3Client(IAwsCredentials credentials)
            : this(AwsRegion.Standard, credentials) { }

        public S3Client(AwsRegion region, IAwsCredentials credentials)
            : base(AwsService.S3, region, credentials) { }

        public void SetTimeout(TimeSpan timeout)
        {
            httpClient.Timeout = timeout;
        }

        // CreateBucketRequest?

        public async Task<ListBucketResult> ListBucketAsync(string bucketName, ListBucketOptions options)
        {
            var request = new ListBucketRequest(region, bucketName, options);

            var responseText = await SendAsync(request).ConfigureAwait(false);          

            return ListBucketResult.ParseXml(responseText);
        }

        #region Multipart Uploads

        public async Task<InitiateMultipartUploadResult> InitiateMultipartUploadAsync(InitiateMultipartUploadRequest request)
        {
            var responseText = await SendAsync(request).ConfigureAwait(false);

            return InitiateMultipartUploadResult.ParseXml(responseText);
        }

        public async Task<UploadPartResult> UploadPartAsync(UploadPartRequest request)
        {
            using (var response = await SendAsync2(request).ConfigureAwait(false))
            {
                return new UploadPartResult(
                    partNumber  : request.PartNumber,
                    uploadId    : request.UploadId,
                    eTag        : response.Headers.ETag.Tag
                );
            }
        }

        public async Task<CompleteMultipartUploadResult> CompleteMultipartUploadAsync(CompleteMultipartUploadRequest request)
        {
            var responseText = await SendAsync(request).ConfigureAwait(false);

            return CompleteMultipartUploadResult.ParseXml(responseText);
        }

        #endregion

        public async Task<PutObjectResult> PutObjectAsync(PutObjectRequest request)
        {
            using (var response = await SendAsync2(request).ConfigureAwait(false))
            {
                return new PutObjectResult(response);
            }
        }

        public async Task<CopyObjectResult> CopyObjectAsync(CopyObjectRequest request)
        {
            var responseText = await SendAsync(request).ConfigureAwait(false);

            return CopyObjectResult.ParseXml(responseText);
        }

        public async Task DeleteObjectAsync(DeleteObjectRequest request)
        {
            using (var response = await SendAsync2(request).ConfigureAwait(false))
            {
                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    throw new Exception("Expected 204");
                }
            }
        }

        public async Task<DeleteResult> DeleteObjectsAsync(BatchDeleteRequest request)
        {
            var responseText = await SendAsync(request).ConfigureAwait(false);
            
            return DeleteResult.Parse(responseText);
        }

        public async Task<RestoreObjectResult> RestoreObjectAsync(RestoreObjectRequest request)
        {
            using (var response = await SendAsync2(request).ConfigureAwait(false))
            {
                return new RestoreObjectResult(response);
            }
        }

        public async Task<S3Object> GetObjectAsync(GetObjectRequest request)
        {
            var response = await SendAsync2(request).ConfigureAwait(false);

            return new S3Object(request.Key, response);
        }

        public async Task<S3ObjectInfo> GetObjectHeadAsync(ObjectHeadRequest request)
        {
            using (var response = await SendAsync2(request).ConfigureAwait(false))
            {
                return new S3ObjectInfo(
                    bucketName : request.BucketName,
                    name       : request.Key,
                    response   : response
                );
            }
        }

        private async Task<HttpResponseMessage> SendAsync2(HttpRequestMessage request)
        {
            await SignAsync(request).ConfigureAwait(false);

            var response = await httpClient.SendAsync(request).ConfigureAwait(false);
            
            if (!response.IsSuccessStatusCode)
            {
                using (response)
                {
                    throw await GetException(response).ConfigureAwait(false);
                }
            }

            return response;

        }

        #region Helpers

        protected override async Task<Exception> GetException(HttpResponseMessage response)
        {
            var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                // Consider limiting to KeyNotFound?
                
                throw StorageException.NotFound(response.RequestMessage.RequestUri.AbsolutePath.TrimStart('/'));
            }

            if (responseText.Contains("<Error>"))
            {
                var error = S3Error.ParseXml(responseText);

                throw new S3Exception(error, response.StatusCode, responseText);
            }

            throw new S3Exception("Unexpected S3 error. " + response.StatusCode + ":" + responseText, response.StatusCode);
        }

        #endregion       
    }
}