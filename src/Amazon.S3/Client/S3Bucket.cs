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

    public sealed class S3Bucket : IBucket, IReadOnlyBucket
    {
        private readonly S3Client client;
        private readonly string bucketName;

        private static readonly RetryPolicy retryPolicy = RetryPolicy.ExponentialBackoff(
             initialDelay: TimeSpan.FromMilliseconds(100),
             maxDelay: TimeSpan.FromSeconds(3),
             maxRetries: 5
        );

        public S3Bucket(AwsRegion region, string bucketName, IAwsCredential credential)
            : this(bucketName, new S3Client(region, credential)) { }

        public S3Bucket(string bucketName, S3Client client)
        {
            this.bucketName = bucketName ?? throw new ArgumentNullException(nameof(bucketName));
            this.client     = client     ?? throw new ArgumentNullException(nameof(client));
        }

        public string Name => bucketName;

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
                Prefix = prefix,
                ContinuationToken = continuationToken,
                MaxKeys = take
            };

            var result = await client.ListBucketAsync(bucketName, request).ConfigureAwait(false);

            return result.Items;
        }

        public async Task<IBlob> GetAsync(string key)
        {
            var request = new GetObjectRequest(
                host       : client.Host,
                bucketName : bucketName,
                objectName : key
            );

            return await client.GetObjectAsync(request).ConfigureAwait(false);
        }

        public string GetPresignedUrl(string key, TimeSpan expiresIn)
        {
            var request = new GetPresignedUrlRequest(client.Host, client.Region, bucketName, key, expiresIn);

            return client.GetPresignedUrl(in request);
        }

        // If-Modified-Since                    OR 304
        // If-None-Match        -- ETag         OR 304
        public async Task<IBlobResult> GetAsync(string key, GetBlobOptions options)
        {
            #region Preconditions

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            #endregion

            var request = new GetObjectRequest(
                host       : client.Host,
                bucketName : bucketName,
                objectName : key
            );

            if (options.IfModifiedSince != null)
            {
                request.IfModifiedSince = options.IfModifiedSince;
            }

            if (options.EncryptionKey != null)
            {
                request.SetCustomerEncryptionKey(new ServerSideEncryptionKey(options.EncryptionKey));
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

        public async Task<IReadOnlyDictionary<string, string>> GetPropertiesAsync(string key)
        {
            #region Preconditions

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            #endregion

            var request = new ObjectHeadRequest(client.Host, bucketName, key: key);

            var result = await client.GetObjectHeadAsync(request).ConfigureAwait(false);

            return result.Properties;
        }

        public Task<RestoreObjectResult> InitiateRestoreAsync(string key, int days)
        {
            #region Preconditions

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            #endregion

            var request = new RestoreObjectRequest(client.Host, bucketName, key) {
                Days = days
            };

            return client.RestoreObjectAsync(request);
        }

        public async Task<CopyObjectResult> PutAsync(
            string key,
            S3ObjectLocation sourceLocation,
            IReadOnlyDictionary<string, string> metadata = null)
        {
            #region Preconditions

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            #endregion

            var retryCount = 0;
            Exception lastException = null;

            while (retryPolicy.ShouldRetry(retryCount))
            {
                try
                {
                    return await PutInternalAsync(key, sourceLocation, metadata).ConfigureAwait(false);
                }
                catch (S3Exception ex) when (ex.IsTransient)
                {
                    lastException = ex;
                }

                retryCount++;

                await Task.Delay(retryPolicy.GetDelay(retryCount)).ConfigureAwait(false);
            }

            throw new S3Exception($"Unrecoverable exception copying '{sourceLocation}' to '{key}'", lastException);
        }

        private Task<CopyObjectResult> PutInternalAsync(
            string key,
            S3ObjectLocation sourceLocation,
            IReadOnlyDictionary<string, string> properties = null)
        {
            var request = new CopyObjectRequest(
                host   : client.Host,
                source : sourceLocation,
                target : new S3ObjectLocation(bucketName, key)
            );

            SetHeaders(request, properties);

            return client.CopyObjectAsync(request);
        }

        public Task PutAsync(IBlob blob)
        {
            return PutAsync(blob, new PutBlobOptions());
        }

        public async Task PutAsync(IBlob blob, PutBlobOptions options)
        {
            #region Preconditions

            if (blob == null)
                throw new ArgumentNullException(nameof(blob));

            if (blob.Key == null)
                throw new ArgumentNullException("blob.Key");

            #endregion

            // TODO: Chunked upload

            var stream = await blob.OpenAsync().ConfigureAwait(false);

            #region Stream conditions

            if (stream.Length == 0)
                throw new ArgumentException("May not be empty", nameof(blob));

            // Ensure we're at the start of the stream
            if (stream.CanSeek && stream.Position != 0)
                throw new ArgumentException($"Must be 0. Was {stream.Position}.", paramName: "blob.Position");

            #endregion

            var request = new PutObjectRequest(client.Host, bucketName, blob.Key);

            request.SetStream(stream);

            // Server side encrpytion
            if (options.EncryptionKey != null)
            {
                request.SetCustomerEncryptionKey(new ServerSideEncryptionKey(options.EncryptionKey));
            }

            SetHeaders(request, blob.Properties);

            await client.PutObjectAsync(request).ConfigureAwait(false);
        }

        public async Task DeleteAsync(string name)
        {
            #region Preconditions

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            #endregion

            var request = new DeleteObjectRequest(client.Host, bucketName, name);

            await client.DeleteObjectAsync(request).ConfigureAwait(false);
        }

        public Task DeleteAsync(string key, string version)
        {
            #region Preconditions

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            #endregion

            var request = new DeleteObjectRequest(client.Host, bucketName, key, version);

            return client.DeleteObjectAsync(request);
        }

        public async Task DeleteAsync(string[] keys)
        {
            #region Preconditions

            if (keys == null)
                throw new ArgumentNullException(nameof(keys));

            #endregion

            var batch = new DeleteBatch(keys);

            var request = new BatchDeleteRequest(client.Host, bucketName, batch);

            var result = await client.DeleteObjectsAsync(request).ConfigureAwait(false);

            if (result.HasErrors)
            {
                throw new Exception(result.Errors[0].Message);
            }

            if (result.Deleted.Length != keys.Length)
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

        public async Task<IUpload> StartUploadAsync(string key, IReadOnlyDictionary<string, string> properties)
        {
            #region Preconditions

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            #endregion

            var request = new InitiateMultipartUploadRequest(client.Host, bucketName, key) {
                Content = new StringContent(string.Empty)
            };

            SetHeaders(request, properties);

            return await client.InitiateMultipartUploadAsync(request).ConfigureAwait(false);
        }

        public async Task<IUploadBlock> UploadBlock(IUpload upload, int number, Stream stream)
        {
            #region Preconditions

            if (upload == null)
                throw new ArgumentNullException(nameof(upload));

            #endregion

            var request = new UploadPartRequest(client.Host, upload, number);

            request.SetStream(stream);

            return await client.UploadPartAsync(request).ConfigureAwait(false);
        }

        public async Task CompleteUploadAsync(IUpload upload, IUploadBlock[] blocks)
        {
            var request = new CompleteMultipartUploadRequest(client.Host, upload, blocks);

            await client.CompleteMultipartUploadAsync(request).ConfigureAwait(false);
        }

        public async Task CancelUploadAsync(IUpload upload)
        {
            #region Preconditions

            if (upload == null)
                throw new ArgumentNullException(nameof(upload));

            #endregion

            var request = new AbortMultipartUploadRequest(
                host       : client.Host, 
                bucketName : upload.BucketName, 
                key        : upload.ObjectName,
                uploadId   : upload.UploadId
            );

            await client.AbortMultipartUploadAsync(request).ConfigureAwait(false);
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
                    case "Content-Encoding" :
                        request.Content.Headers.ContentEncoding.Add(item.Value); break;
                    case "Content-Type":
                        request.Content.Headers.ContentType = new MediaTypeHeaderValue(item.Value); break;

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