using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Security.Cryptography;

using Carbon.Storage;

namespace Amazon.S3
{
    using Scheduling;
 
    public class S3Bucket : IBucket
    {
        private readonly S3Client s3;
        private readonly string bucketName;

        private static readonly RetryPolicy retryPolicy = RetryPolicy.ExponentialBackoff(
             initialDelay : TimeSpan.FromMilliseconds(100),
             maxDelay     : TimeSpan.FromSeconds(3),
             maxRetries   : 5);

        public S3Bucket(string bucketName, AwsCredentials credentials)
            : this(AwsRegion.Standard, bucketName, credentials) { }
              
        public S3Bucket(AwsRegion region, string bucketName, AwsCredentials credentials)
        {
            #region Preconditions

            if (bucketName == null)
                throw new ArgumentNullException(nameof(bucketName));

            if (credentials == null)
                throw new ArgumentNullException(nameof(credentials));

            #endregion

            this.bucketName = bucketName;

            this.s3 = new S3Client(region, credentials);
        }

        public S3Bucket WithTimeout(TimeSpan timeout)
        {
            s3.SetTimeout(timeout);

            return this;
        }

        public Task<IReadOnlyList<IBlob>> ListAsync(string prefix)
            => ListAsync(prefix, null, 1000);
        
        public async Task<IReadOnlyList<IBlob>> ListAsync(string prefix, string continuationToken, int take = 1000)
        {
            var request = new ListBucketOptions {
                Prefix            = prefix,
                ContinuationToken = continuationToken,
                MaxKeys           = take
            };

            var result = await s3.ListBucket(bucketName, request).ConfigureAwait(false);

            return result.Items;
        }

        public async Task<IBlob> GetRange(string key, long start, long end)
        {
            var request = new GetObjectRequest(s3.Region, bucketName, key: key);

            request.SetRange(start, end);

            return await s3.GetObject(request).ConfigureAwait(false);
        }

        public async Task<IBlob> GetAsync(string key)
        {
            var request = new GetObjectRequest(s3.Region, bucketName, key: key);

            return await s3.GetObject(request).ConfigureAwait(false);
        }

        public async Task<IBlob> GetBufferedAsync(string key)
        {
            var request = new GetObjectRequest(s3.Region, bucketName, key: key);

            request.CompletionOption = HttpCompletionOption.ResponseContentRead;

            return await s3.GetObject(request).ConfigureAwait(false);
        }

        public async Task<IDictionary<string, string>> GetMetadataAsync(string key)
        {
            var headRequest = new ObjectHeadRequest(s3.Region, bucketName, key: key);

            var result = await s3.GetObjectHead(headRequest).ConfigureAwait(false);

            return result.Headers;
        }

        public Task<RestoreObjectResult> InitiateRestoreAsync(string key, int days)
            => s3.RestoreObject(new RestoreObjectRequest(s3.Region, bucketName, key, days));

        public async Task<CopyObjectResult> PutAsync(string key, S3ObjectLocation sourceLocation, IDictionary<string, string> metadata = null)
        {
            var retryCount = 0;
            Exception lastException = null;

            while (retryPolicy.ShouldRetry(retryCount))
            {
                try
                {
                    return await _PutAsync(key, sourceLocation, metadata).ConfigureAwait(false);
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

        private Task<CopyObjectResult> _PutAsync(string key, S3ObjectLocation sourceLocation, IDictionary<string, string> metadata = null)
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

                        default: request.Headers.Add(item.Key, item.Value); break;
                    }
                }
            }

            return s3.CopyObject(request);
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
            
            if (stream.CanSeek)
            {
                // Add a checksum (base64 encoded) to verify the integrity of the transfer
                request.Content.Headers.ContentMD5 = ComputeMD5(stream);
            }

            await s3.PutObject(request).ConfigureAwait(false);
        }

        const int chunkSize = 1024 * 1025 * 5; // 5MB

        private byte[] ComputeMD5(Stream stream)
        {
            using (var md5 = MD5.Create())
            {
                var data = md5.ComputeHash(stream);

                stream.Position = 0;

                return data;
            }
        }

        public async Task PutBigAsync(string key, Blob blob)
        {
            var stream = blob.Open();

            var parts = new List<UploadPartResult>();

            var initiateRequest = new InitiateMultipartUploadRequest(s3.Region, bucketName, key);

            initiateRequest.Content = new StringContent("");

            foreach (var item in blob.Metadata)
            {
                switch (item.Key)
                {
                    case "Content-Encoding" : initiateRequest.Content.Headers.ContentEncoding.Add(item.Value); break;
                    case "Content-Type"     : initiateRequest.Content.Headers.ContentType = new MediaTypeHeaderValue(item.Value); break;

                    default: initiateRequest.Headers.Add(item.Key, item.Value); break;
                }
            }

            var session = await s3.InitiateMultipartUpload(initiateRequest).ConfigureAwait(false);

            using (var slicer = new BlobSlicer(stream, chunkSize))
            {
                while (!slicer.IsEof)
                {
                    using (var slice = await slicer.ReadAsync().ConfigureAwait(false))
                    {
                        var partRequest = new UploadPartRequest(s3.Region, bucketName, key, parts.Count + 1, session.UploadId);

                        if (slice.Stream.CanSeek)
                        {
                            // Add a checksum (base64 encoded) to verify the integrity of the transfer
                            partRequest.Content.Headers.ContentMD5 = ComputeMD5(slice.Stream);
                        }

                        partRequest.SetStream(slice.Stream);

                        var part = await s3.UploadPart(partRequest).ConfigureAwait(false);

                        parts.Add(part);
                    }
                }

                var completeRequest = new CompleteMultipartUploadRequest(s3.Region, bucketName, key, session.UploadId, parts.ToArray());

                var result = await s3.CompleteMultipartUpload(completeRequest).ConfigureAwait(false);
            }
        }

        public Task DeleteAsync(string key, string version = null)
            => s3.DeleteObject(new DeleteObjectRequest(bucketName, key, version));

        public async Task DeleteAsync(string[] keys)
        {
            var batch = new DeleteBatch(keys);

            var request = new BatchDeleteRequest(s3.Region, bucketName, batch);

            var result = await s3.DeleteObjects(request).ConfigureAwait(false);

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