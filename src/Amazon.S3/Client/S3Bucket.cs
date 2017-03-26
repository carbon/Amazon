using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net.Http;

using Carbon.Storage;

namespace Amazon.S3
{
    using Scheduling;
 
    public class S3Bucket : IBucket, IReadOnlyBucket
    {
        private readonly S3Client client;
        private readonly string bucketName;

        private static readonly RetryPolicy retryPolicy = RetryPolicy.ExponentialBackoff(
             initialDelay : TimeSpan.FromMilliseconds(100),
             maxDelay     : TimeSpan.FromSeconds(3),
             maxRetries   : 5
        );
              
        public S3Bucket(AwsRegion region, string bucketName, IAwsCredential credential)
            : this(bucketName, client: new S3Client(region, credential)) { }

        public S3Bucket(string bucketName, S3Client client)
        {
            this.bucketName = bucketName ?? throw new ArgumentNullException(nameof(bucketName));
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        // TODO: Fetch the region too...

        public S3Bucket(AwsRegion region, string bucketName)
        {
            this.bucketName = bucketName ?? throw new ArgumentNullException(nameof(bucketName));

            var credential = new InstanceRoleCredential(); // this will be fetched on first use

            this.client = new S3Client(region, credential);
        }

        public S3Bucket WithTimeout(TimeSpan timeout)
        {
            client.SetTimeout(timeout);

            return this;
        }

        public Task<IReadOnlyList<IBlob>> ListAsync(string prefix)
        {
            return ListAsync(prefix, null, 1000);
        }

        public async Task<IReadOnlyList<IBlob>> ListAsync(string prefix, string continuationToken, int take = 1000)
        {
            var request = new ListBucketOptions {
                Prefix            = prefix,
                ContinuationToken = continuationToken,
                MaxKeys           = take
            };

            var result = await client.ListBucketAsync(bucketName, request).ConfigureAwait(false);

            return result.Items;
        }

        public async Task<IBlob> GetAsync(string name)
        {
            var request = new GetObjectRequest(client.Region, bucketName, objectName: name);

            return await client.GetObjectAsync(request).ConfigureAwait(false);
        }

        // If-Modified-Since                    OR 304
        // If-None-Match        -- ETag         OR 304
        public async Task<IBlobResult> GetAsync(string name, GetBlobOptions options)
        {
            var request = new GetObjectRequest(client.Region, bucketName, objectName: name);

            if (options.IfModifiedSince != null)
            {
                request.IfModifiedSince = options.IfModifiedSince;
            }

            if (options.IfNoneMatch != null)
            {
                request.IfNoneMatch = options.IfNoneMatch;
            }

            if (options.Range != null)
            {
                request.SetRange(options.Range.Value.Start, options.Range.Value.End);
            }

            if (options.BufferResponse)
            {
                request.CompletionOption = HttpCompletionOption.ResponseContentRead;
            }

            return await client.GetObjectAsync(request).ConfigureAwait(false);
        }

        public async Task<IReadOnlyDictionary<string, string>> GetMetadataAsync(string name)
        {
            var headRequest = new ObjectHeadRequest(client.Region, bucketName, key: name);

            var result = await client.GetObjectHeadAsync(headRequest).ConfigureAwait(false);

            return result.Metadata;
        }

        public Task<RestoreObjectResult> InitiateRestoreAsync(string name, int days)
        {
            var request = new RestoreObjectRequest(client.Region, bucketName, name) {
                Days = days
            };

            return client.RestoreObjectAsync(request);
        }

        public async Task<CopyObjectResult> PutAsync(
            string name, 
            S3ObjectLocation sourceLocation, 
            IReadOnlyDictionary<string, string> metadata = null)
        {
            var retryCount = 0;
            Exception lastException = null;

            while (retryPolicy.ShouldRetry(retryCount))
            {
                try
                {
                    return await PutInternalAsync(name, sourceLocation, metadata).ConfigureAwait(false);
                }
                catch (S3Exception ex) when (ex.IsTransient)
                {
                    lastException = ex;
                }

                retryCount++;

                await Task.Delay(retryPolicy.GetDelay(retryCount)).ConfigureAwait(false);
            }

            throw new S3Exception($"Unrecoverable exception copying '{sourceLocation}' to '{name}'", lastException);
        }

        private Task<CopyObjectResult> PutInternalAsync(
            string name, 
            S3ObjectLocation sourceLocation, 
            IReadOnlyDictionary<string, string> metadata = null)
        {
            var request = new CopyObjectRequest(
                region : client.Region,
                source : sourceLocation,
                target : new S3ObjectLocation(bucketName, name)
            );

            SetHeaders(request, metadata);

            return client.CopyObjectAsync(request);
        }

        public async Task PutAsync(IBlob blob)
        {
            #region Preconditions

            if (blob == null)
                throw new ArgumentNullException(nameof(blob));

            if (blob.Name == null)
                throw new ArgumentNullException("blob.Name");

            #endregion


            // TODO: Chunked upload

            var stream = await blob.OpenAsync().ConfigureAwait(false);

            #region Stream conditions

            if (stream.Length == 0) throw new ArgumentException("May not be empty", nameof(blob));

            // Ensure we're at the start of the stream
            if (stream.CanSeek && stream.Position != 0)
                throw new ArgumentException($"Must be 0. Was {stream.Position}.", paramName: "blob.Position");

            #endregion

            var request = new PutObjectRequest(client.Region, bucketName, blob.Name);
            
            request.SetStream(stream);

            SetHeaders(request, blob.Metadata);

            await client.PutObjectAsync(request).ConfigureAwait(false);
        }

        public Task DeleteAsync(string name, string version = null)
        {
            var request = new DeleteObjectRequest(client.Region, bucketName, name, version);

            return client.DeleteObjectAsync(request);
        }

        public async Task DeleteAsync(string name)
        {
            var request = new DeleteObjectRequest(client.Region, bucketName, name);

            await client.DeleteObjectAsync(request).ConfigureAwait(false);
        }

        public async Task DeleteAsync(string[] names)
        {
            var batch = new DeleteBatch(names);

            var request = new BatchDeleteRequest(client.Region, bucketName, batch);

            var result = await client.DeleteObjectsAsync(request).ConfigureAwait(false);

            if (result.HasErrors)
            {
                throw new Exception(result.Errors[0].Message);
            }

            if (result.Deleted.Count != names.Length)
            {
                throw new Exception("Deleted count not equal to keys.Count");
            }
        }

        #region Uploads

        // TODO: Introduce IUpload in Carbon.Storage

        // TODO: 
        // Carbon.Storage.Uploads
        // - IUpload (bucketName, objectName, uploadId)
        // - IUploadBlock (uploadId, number, state)

        public async Task<IUpload> StartUploadAsync(string name, IReadOnlyDictionary<string, string> metadata)
        {
            var request = new InitiateMultipartUploadRequest(client.Region, bucketName, name) {
                Content = new StringContent(string.Empty)
            };

            SetHeaders(request, metadata);

            return await client.InitiateMultipartUploadAsync(request).ConfigureAwait(false);
        }

        public async Task<IUploadBlock> UploadBlock(IUpload upload, int number, Stream stream)
        {
            var partRequest = new UploadPartRequest(client.Region, upload, number);

            partRequest.SetStream(stream);

            return await client.UploadPartAsync(partRequest).ConfigureAwait(false);
        }

        public async Task CompleteUploadAsync(IUpload upload, IUploadBlock[] blocks)
        {
            var completeRequest = new CompleteMultipartUploadRequest(client.Region, upload, blocks);

            var result = await client.CompleteMultipartUploadAsync(completeRequest).ConfigureAwait(false);
        }

        #endregion

        #region Helpers

        private void SetHeaders(HttpRequestMessage request, IReadOnlyDictionary<string, string> headers)
        {
            if (headers == null) return;

            foreach (var item in headers)
            {
                switch (item.Key)
                {
                    case "Content-Encoding": request.Content.Headers.ContentEncoding.Add(item.Value); break;
                    case "Content-Type": request.Content.Headers.ContentType = new MediaTypeHeaderValue(item.Value); break;

                    // Skip list...
                    case "Accept-Ranges":
                    case "Content-Length":
                    case "Date":
                    case "ETag":
                    case "Server":
                    case "Last-Modified":
                    case "x-amz-expiration":
                    case "x-amz-request-id2":
                    case "x-amz-request-id": continue;

                    default: request.Headers.Add(item.Key, item.Value); break;
                }
            }
        }

        #endregion
    }
}