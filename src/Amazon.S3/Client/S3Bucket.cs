﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Amazon.S3.Helpers;
using Amazon.Scheduling;

using Carbon.Storage;

namespace Amazon.S3
{
    public sealed class S3Bucket : IBucket, IReadOnlyBucket
    {
        private readonly S3Client _client;
        private readonly string _bucketName;

        private static readonly RetryPolicy retryPolicy = RetryPolicy.ExponentialBackoff(
             initialDelay : TimeSpan.FromMilliseconds(50),
             maxDelay     : TimeSpan.FromSeconds(3),
             maxRetries   : 4
        );

        public S3Bucket(AwsRegion region, string bucketName, IAwsCredential credential)
            : this(bucketName, new S3Client(region, credential)) { }

        public S3Bucket(string bucketName, S3Client client)
        {
            _bucketName = bucketName ?? throw new ArgumentNullException(nameof(bucketName));
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public string Name => _bucketName;

        public S3Bucket WithTimeout(TimeSpan timeout)
        {
            _client.WithTimeout(timeout);

            return this;
        }

        public Task<IReadOnlyList<IBlob>> ListAsync(string? prefix = null)
        {
            return ListAsync(prefix, null, 1000);
        }

        public async Task<IReadOnlyList<IBlob>> ListAsync(string? prefix, string? continuationToken, int take = 1000)
        {
            var request = new ListBucketOptions {
                Prefix = prefix,
                ContinuationToken = continuationToken,
                MaxKeys = take
            };

            var result = await _client.ListBucketAsync(_bucketName, request).ConfigureAwait(false);

            return result.Items;
        }

        public async Task<IBlob> GetAsync(string key, CancellationToken cancellationToken = default)
        {
            int retryCount = 0;
            Exception lastException;

            do
            {
                if (retryCount > 0)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    await Task.Delay(retryPolicy.GetDelay(retryCount), cancellationToken).ConfigureAwait(false);
                }

                try
                {
                    var request = new GetObjectRequest(
                        host       : _client.Host,
                        bucketName : _bucketName,
                        key        : key
                    );

                    return await _client.GetObjectAsync(request, cancellationToken).ConfigureAwait(false);
                }
                catch (S3Exception ex) when (ex.IsTransient)
                {
                    lastException = ex;
                }

                retryCount++;

            }
            while (retryPolicy.ShouldRetry(retryCount));

            throw lastException;
        }

        public string GetPresignedUrl(string key, TimeSpan expiresIn, string method = "GET")
        {
            var request = new GetPresignedUrlRequest(method, _client.Host, _client.Region, _bucketName, key, expiresIn);

            return _client.GetPresignedUrl(request);
        }

        // If-Modified-Since                    OR 304
        // If-None-Match        -- ETag         OR 304

        public Task<IBlobResult> GetAsync(string key, GetBlobOptions options)
        {
            return GetAsync(key, options, default);
        }

        public async Task<IBlobResult> GetAsync(string key, GetBlobOptions options, CancellationToken cancellationToken)
        {
            if (options is null) throw new ArgumentNullException(nameof(options));

            int retryCount = 0;
            Exception lastException;

            do
            {
                if (retryCount > 0)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    
                    await Task.Delay(retryPolicy.GetDelay(retryCount), cancellationToken).ConfigureAwait(false);
                }

                try
                {
                    var request = ConstructGetRequest(key, options);

                    return await _client.GetObjectAsync(request, cancellationToken).ConfigureAwait(false);
                }
                catch (S3Exception ex) when (ex.IsTransient)
                {
                    lastException = ex;
                }

                retryCount++;

            }
            while (retryPolicy.ShouldRetry(retryCount));

            throw lastException;
        }

        public Task<IReadOnlyDictionary<string, string>> GetPropertiesAsync(string key)
        {
            return GetPropertiesAsync(key, default);
        }

        public async Task<IReadOnlyDictionary<string, string>> GetPropertiesAsync(string key, CancellationToken cancellationToken)
        {
            int retryCount = 0;
            Exception lastException;

            do
            {
                if (retryCount > 0)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    await Task.Delay(retryPolicy.GetDelay(retryCount), cancellationToken).ConfigureAwait(false);
                }

                try
                {
                    var request = new ObjectHeadRequest(_client.Host, _bucketName, key);

                    using var result = await _client.GetObjectHeadAsync(request, cancellationToken).ConfigureAwait(false);

                    return result.Properties;
                }
                catch (S3Exception ex) when (ex.IsTransient)
                {
                    lastException = ex;
                }

                retryCount++;

            }
            while (retryPolicy.ShouldRetry(retryCount));

            throw lastException;
        }

        public Task<RestoreObjectResult> InitiateRestoreAsync(string key, int days)
        {
            var request = new RestoreObjectRequest(_client.Host, _bucketName, key) {
                Days = days
            };

            return _client.RestoreObjectAsync(request);
        }

        public async Task<CopyObjectResult> PutAsync(
            string destinationKey,
            S3ObjectLocation sourceLocation,
            IReadOnlyDictionary<string, string>? metadata = null)
        { 
            if (destinationKey is null)
            {
                throw new ArgumentNullException(nameof(destinationKey));
            }
         
            int retryCount = 0;
            Exception lastException;

            do
            {
                if (retryCount > 0)
                {
                    await Task.Delay(retryPolicy.GetDelay(retryCount)).ConfigureAwait(false);
                }

                var request = new CopyObjectRequest(
                    host   : _client.Host,
                    source : sourceLocation,
                    target : new S3ObjectLocation(_bucketName, destinationKey)
                );

                if (metadata != null)
                {
                    request.MetadataDirective = MetadataDirectiveValue.Replace;

                    request.UpdateHeaders(metadata);
                }

                try
                {
                    return await _client.CopyObjectAsync(request).ConfigureAwait(false);
                }
                catch (S3Exception ex) when (ex.IsTransient)
                {
                    lastException = ex;
                }

                retryCount++;
            }
            while (retryPolicy.ShouldRetry(retryCount));

            throw new S3Exception($"Error copying '{sourceLocation}' to '{destinationKey}'", lastException);
        }

        private static readonly PutBlobOptions defaultPutOptions = new ();

        public Task PutAsync(IBlob blob, CancellationToken cancelationToken = default)
        {
            return PutAsync(blob, defaultPutOptions, cancelationToken);
        }

        public async Task PutAsync(
            IBlob blob,
            PutBlobOptions options, 
            CancellationToken cancellationToken = default)
        {
            if (blob is null)
                throw new ArgumentNullException(nameof(blob));

            if (blob.Key is null)
                throw new ArgumentException("Missing key", nameof(blob));

            // TODO: Chunked upload

            Stream stream = await blob.OpenAsync().ConfigureAwait(false);

            if (stream.Length is 0)
                throw new ArgumentException("May not be empty", nameof(blob));

            // Ensure we're at the start of the stream
            if (stream.CanSeek && stream.Position != 0)
                throw new ArgumentException($"Invalid position. Expected 0. Was {stream.Position}.", nameof(blob));


            int retryCount = 0;
            Exception lastException;

            do
            {
                if (retryCount > 0)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    await Task.Delay(retryPolicy.GetDelay(retryCount), cancellationToken).ConfigureAwait(false);
                }

                var request = new PutObjectRequest(_client.Host, _bucketName, blob.Key);

                request.SetStream(stream);

                // Server side encrpytion
                if (options.EncryptionKey != null)
                {
                    request.SetCustomerEncryptionKey(new ServerSideEncryptionKey(options.EncryptionKey));
                }

                if (options.Tags is { Count: > 0 })
                {
                    request.SetTagSet(options.Tags);
                }

                request.UpdateHeaders(blob.Properties);

                try
                {
                    await _client.PutObjectAsync(request, cancellationToken).ConfigureAwait(false);

                    return;
                }
                catch (S3Exception ex) when (ex.IsTransient)
                {
                    lastException = ex;
                }
                
                stream.Position = 0; // reset the stream position
                
                retryCount++;
            }
            while (retryPolicy.ShouldRetry(retryCount));
            
            throw lastException;
        }

        public async Task DeleteAsync(string name)
        {
            await _client.DeleteObjectAsync(
                new DeleteObjectRequest(_client.Host, _bucketName, name)
            ).ConfigureAwait(false);
        }

        public async Task DeleteAsync(string key, string version)
        {
            var request = new DeleteObjectRequest(_client.Host, _bucketName, key, version);

            await _client.DeleteObjectAsync(request).ConfigureAwait(false);
        }

        public async Task<DeleteResult> DeleteAsync(IReadOnlyList<string> keys)
        {
            var batch = new DeleteBatch(keys, quite: true);

            var request = new DeleteObjectsRequest(_client.Host, _bucketName, batch);

            var result = await _client.DeleteObjectsAsync(request).ConfigureAwait(false);

            if (result.Errors is { Length: > 0 } errors)
            {
                throw new Exception(errors[0].Message);
            }

            return result;
        }

        #region Uploads

        public async Task<IUpload> InitiateUploadAsync(string key, IReadOnlyDictionary<string, string> properties)
        {
            int retryCount = 0;
            Exception lastException;

            do
            {
                if (retryCount > 0)
                {
                    await Task.Delay(retryPolicy.GetDelay(retryCount)).ConfigureAwait(false);
                }

                var request = new InitiateMultipartUploadRequest(_client.Host, _bucketName, key, properties);

                try
                {
                    return await _client.InitiateMultipartUploadAsync(request).ConfigureAwait(false);
                }
                catch (S3Exception ex) when (ex.IsTransient)
                {
                    lastException = ex;
                }

                retryCount++;
            }
            while (retryPolicy.ShouldRetry(retryCount));

            throw lastException;
        }

        public async Task<IUploadBlock> UploadPartAsync(IUpload upload, int number, Stream stream)
        {
            if (upload is null) 
                throw new ArgumentNullException(nameof(upload));

            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            int retryCount = 0;
            Exception lastException;

            do
            {
                if (retryCount > 0)
                {
                    await Task.Delay(retryPolicy.GetDelay(retryCount)).ConfigureAwait(false);
                }

                var request = new UploadPartRequest(
                    host       : _client.Host,
                    bucketName : upload.BucketName,
                    key        : upload.ObjectName,
                    uploadId   : upload.UploadId,
                    partNumber : number
                );

                request.SetStream(stream);

                try
                {
                    return await _client.UploadPartAsync(request).ConfigureAwait(false);
                }
                catch (S3Exception ex) when (ex.IsTransient)
                {
                    lastException = ex;
                }

                stream.Position = 0;

                retryCount++;
            }
            while (retryPolicy.ShouldRetry(retryCount));

            throw lastException;
        }
    
        public async Task FinalizeUploadAsync(IUpload upload, IUploadBlock[] blocks)
        {
            int retryCount = 0;
            Exception lastException;

            do
            {
                if (retryCount > 0)
                {
                    await Task.Delay(retryPolicy.GetDelay(retryCount)).ConfigureAwait(false);
                }

                try
                {
                    var request = new CompleteMultipartUploadRequest(_client.Host, upload, blocks);

                    await _client.CompleteMultipartUploadAsync(request).ConfigureAwait(false);

                    return;
                }
                catch (S3Exception ex) when (ex.IsTransient)
                {
                    lastException = ex;
                }

                retryCount++;
            }
            while (retryPolicy.ShouldRetry(retryCount));

            throw lastException;
        }

        public async Task CancelUploadAsync(IUpload upload)
        {
            if (upload is null) throw new ArgumentNullException(nameof(upload));

            var request = new AbortMultipartUploadRequest(
                host       : _client.Host, 
                bucketName : upload.BucketName, 
                key        : upload.ObjectName,
                uploadId   : upload.UploadId
            );

            await _client.AbortMultipartUploadAsync(request).ConfigureAwait(false);
        }

        #endregion

        #region Helpers

        private GetObjectRequest ConstructGetRequest(string key, GetBlobOptions options)
        {
            var request = new GetObjectRequest(
                host        : _client.Host,
                bucketName  : _bucketName,
                key         : key
            );

            if (options.IfModifiedSince is DateTimeOffset ifModifiedSince)
            {
                request.IfModifiedSince = ifModifiedSince;
            }

            if (options.EncryptionKey is byte[] encryptionKey)
            {
                request.SetCustomerEncryptionKey(new ServerSideEncryptionKey(encryptionKey));
            }

            if (options.IfNoneMatch is string ifNoneMatch)
            {
                request.IfNoneMatch = ifNoneMatch;
            }

            if (options.Range is ByteRange range)
            {
                request.SetRange(range.Start, range.End);
            }

            if (options.BufferResponse)
            {
                request.CompletionOption = HttpCompletionOption.ResponseContentRead;
            }

            return request;
        }

        #endregion
    }
}

// NOTES:
// As of PR #19082, HttpClient no longer disposes the HttpContent request object.
// https://github.com/dotnet/corefx/issues/21790