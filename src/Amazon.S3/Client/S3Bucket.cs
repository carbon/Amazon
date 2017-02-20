using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net.Http;

using Carbon.Storage;

namespace Amazon.S3
{
    using Scheduling;
 
    public class S3Bucket : IBucket, IReadOnlyBucket
    {
        private readonly S3Client s3;
        private readonly string bucketName;

        private static readonly RetryPolicy retryPolicy = RetryPolicy.ExponentialBackoff(
             initialDelay : TimeSpan.FromMilliseconds(100),
             maxDelay     : TimeSpan.FromSeconds(3),
             maxRetries   : 5);

        public S3Bucket(string bucketName, IAwsCredentials credentials)
            : this(AwsRegion.Standard, bucketName, credentials) { }
              
        public S3Bucket(AwsRegion region, string bucketName, IAwsCredentials credentials)
            : this(bucketName, client: new S3Client(region, credentials)) { }

        public S3Bucket(string bucketName, S3Client client)
        {
            #region Preconditions

            if (bucketName == null)
                throw new ArgumentNullException(nameof(bucketName));

            if (client == null)
                throw new ArgumentNullException(nameof(client));

            #endregion

            this.bucketName = bucketName;

            this.s3 = client;
        }

        public S3Bucket WithTimeout(TimeSpan timeout)
        {
            s3.SetTimeout(timeout);

            return this;
        }

        public Task<IReadOnlyList<IBlob>> ListAsync(string prefix) => ListAsync(prefix, null, 1000);
        
        public async Task<IReadOnlyList<IBlob>> ListAsync(string prefix, string continuationToken, int take = 1000)
        {
            var request = new ListBucketOptions {
                Prefix            = prefix,
                ContinuationToken = continuationToken,
                MaxKeys           = take
            };

            var result = await s3.ListBucketAsync(bucketName, request).ConfigureAwait(false);

            return result.Items;
        }

        public async Task<IBlob> GetRangeAsync(string key, long start, long end)
        {
            var request = new GetObjectRequest(s3.Region, bucketName, key: key);

            request.SetRange(start, end);

            return await s3.GetObjectAsync(request).ConfigureAwait(false);
        }

        public async Task<IBlob> GetAsync(string key)
        {
            var request = new GetObjectRequest(s3.Region, bucketName, key: key);

            return await s3.GetObjectAsync(request).ConfigureAwait(false);
        }

        public async Task<IBlob> GetBufferedAsync(string key)
        {
            var request = new GetObjectRequest(s3.Region, bucketName, key: key);

            request.CompletionOption = HttpCompletionOption.ResponseContentRead;

            return await s3.GetObjectAsync(request).ConfigureAwait(false);
        }

        public async Task<IDictionary<string, string>> GetMetadataAsync(string key)
        {
            var headRequest = new ObjectHeadRequest(s3.Region, bucketName, key: key);

            var result = await s3.GetObjectHeadAsync(headRequest).ConfigureAwait(false);

            return result.Headers;
        }

        public Task<RestoreObjectResult> InitiateRestoreAsync(string key, int days) => 
            s3.RestoreObjectAsync(new RestoreObjectRequest(s3.Region, bucketName, key, days));

        public async Task<CopyObjectResult> PutAsync(string key, S3ObjectLocation sourceLocation, IDictionary<string, string> metadata = null)
        {
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

        private Task<CopyObjectResult> PutInternalAsync(string key, S3ObjectLocation sourceLocation, IDictionary<string, string> metadata = null)
        {
            var request = new CopyObjectRequest(
                region : s3.Region,
                source : sourceLocation,
                target : new S3ObjectLocation(bucketName, key)
            );

            if (metadata != null)
            {
                foreach (var item in metadata)
                {
                    switch (item.Key)
                    {
                        case "Content-Encoding" : request.Content.Headers.ContentEncoding.Add(item.Value); break;
                        case "Content-Type"     : request.Content.Headers.ContentType = new MediaTypeHeaderValue(item.Value); break;
                        default                 : request.Headers.Add(item.Key, item.Value); break;
                    }
                }
            }

            return s3.CopyObjectAsync(request);
        }

        public async Task PutAsync(string key, Blob blob)
        {
            #region Preconditions

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (blob == null)
                throw new ArgumentNullException(nameof(blob));

            #endregion

            /*
            if (blob.Length > chunkSize * 20) // 100MB
            {
                await UploadBigAsync(key, blob).ConfigureAwait(false);

                return;     
            }
            */

            var stream = blob.Open();

            #region Stream conditions

            if (stream.Length == 0) throw new ArgumentException("May not be 0", "blob.Length");

            // Ensure we're at the start of the stream
            if (stream.CanSeek && stream.Position != 0)
                throw new ArgumentException($"Must be 0. Was {stream.Position}.", paramName: "blob.Position");

            #endregion

            var request = new PutObjectRequest(s3.Region, bucketName, key);
            
            request.SetStream(stream);

            foreach (var item in blob.Metadata)
            {
                switch (item.Key)
                {
                    case "Content-Encoding" : request.Content.Headers.ContentEncoding.Add(item.Value); break;
                    case "Content-Type"     : request.Content.Headers.ContentType = new MediaTypeHeaderValue(item.Value); break;

                    case "Accept-Ranges"    : continue;
                    case "Content-Length"   : continue;

                    case "Date": continue;
                    case "ETag": continue;
                    case "Server": continue;
                    case "Last-Modified": continue;

                    case "x-amz-request-id": continue;

                    default                 : request.Headers.Add(item.Key, item.Value); break;
                }
            }

            await s3.PutObjectAsync(request).ConfigureAwait(false);
        }

        const int chunkSize = 1024 * 1025 * 5; // 5MB

        public async Task PutBigAsync(string key, Blob blob)
        {
            var stream = blob.Open();

            var parts = new List<UploadPartResult>();

            var initiateRequest = new InitiateMultipartUploadRequest(s3.Region, bucketName, key);

            initiateRequest.Content = new StringContent(string.Empty);

            foreach (var item in blob.Metadata)
            {
                switch (item.Key)
                {
                    case "Content-Encoding" : initiateRequest.Content.Headers.ContentEncoding.Add(item.Value); break;
                    case "Content-Type"     : initiateRequest.Content.Headers.ContentType = new MediaTypeHeaderValue(item.Value); break;

                    default: initiateRequest.Headers.Add(item.Key, item.Value); break;
                }
            }

            var session = await s3.InitiateMultipartUploadAsync(initiateRequest).ConfigureAwait(false);

            using (var slicer = new BlobSlicer(stream, chunkSize))
            {
                while (!slicer.IsEof)
                {
                    using (var slice = await slicer.ReadAsync().ConfigureAwait(false))
                    {
                        var partRequest = new UploadPartRequest(s3.Region, bucketName, key, parts.Count + 1, session.UploadId);

                        partRequest.SetStream(slice.Stream);

                        var part = await s3.UploadPartAsync(partRequest).ConfigureAwait(false);

                        parts.Add(part);
                    }
                }

                var completeRequest = new CompleteMultipartUploadRequest(s3.Region, bucketName, key, session.UploadId, parts.ToArray());

                var result = await s3.CompleteMultipartUploadAsync(completeRequest).ConfigureAwait(false);
            }
        }

        public Task DeleteAsync(string key, string version = null) => 
            s3.DeleteObjectAsync(new DeleteObjectRequest(s3.Region, bucketName, key, version));

        public async Task DeleteAsync(string[] keys)
        {
            var batch = new DeleteBatch(keys);

            var request = new BatchDeleteRequest(s3.Region, bucketName, batch);

            var result = await s3.DeleteObjectsAsync(request).ConfigureAwait(false);

            if (result.HasErrors)
            {
                throw new Exception(result.Errors[0].Message);
            }

            if (result.Deleted.Count != keys.Length)
            {
                throw new Exception("Deleted count not equal to keys.Count");
            }
        }
    }
}