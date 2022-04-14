﻿using System.Buffers;
using System.IO;
using System.Net.Http;
using System.Threading;

using Carbon.Storage;

using Microsoft.Win32.SafeHandles;

namespace Amazon.S3.Extensions;

public static class S3ClientExtensions
{    
    private const int bufferSize = 65_536; // 64KB  

    public static async Task<S3ObjectInfo> DownloadAsync(
        this S3Client s3Client,
        string bucketName,
        string objectKey,
        string destinationPath,
        S3DownloadOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        options ??= S3DownloadOptions.Default;

        var metadata = await s3Client.GetObjectHeadAsync(new ObjectHeadRequest(s3Client.Host, bucketName, objectKey), cancellationToken);

        long total = metadata.ContentLength;

        var ranges = GetRanges(total, options.ChunkSize);
        Exception? exception = null;

        var completedTaskSource = new TaskCompletionSource();

        using var _semaphore = new SemaphoreSlim(options.MaxThreadCount, options.MaxThreadCount);

        using var fileHandle = File.OpenHandle(
            path              : destinationPath,
            mode              : FileMode.CreateNew,
            access            : FileAccess.ReadWrite, FileShare.None, 
            options           : FileOptions.WriteThrough | FileOptions.Asynchronous, 
            preallocationSize : total
        );

        int remainingTaskCount = ranges.Length;

        for (int i = 0; i < ranges.Length; i++)
        {
            await _semaphore.WaitAsync(cancellationToken);                

            if (exception != null)
            {
                throw exception;
            }

            var range = ranges[i];

            var task = s3Client.DownloadByteRange(bucketName, objectKey, range, fileHandle, cancellationToken).ContinueWith(task => {
                exception = task.Exception?.InnerException ?? task.Exception;

                _semaphore.Release();

                if (Interlocked.Decrement(ref remainingTaskCount) is 0)
                {
                    completedTaskSource.SetResult();
                }
            });
        }

        if (exception != null)
        {
            throw exception;
        }

        await completedTaskSource.Task.ConfigureAwait(false);

        if (exception != null)
        {
            throw exception;
        }

        return metadata;
    }

    private static async Task DownloadByteRange(
        this S3Client s3, 
        string bucketName, 
        string objectKey, 
        ByteRange range, 
        SafeFileHandle fileHandle,
        CancellationToken cancellationToken)
    {       
        var request = new GetObjectRequest(s3.Host, bucketName, objectKey) {
            CompletionOption = HttpCompletionOption.ResponseHeadersRead
        };

        request.SetRange(range.Start, range.End);

        // TODO: Retry on failure.

        using var result = await s3.GetObjectAsync(request, cancellationToken).ConfigureAwait(false);

        using var stream = await result.OpenAsync();

        long fileOffset = range.Start!.Value;

        int position = 0;
        long remaining = result.ContentLength;

        long rangeLength = (range.End!.Value - range.Start!.Value) + 1;

        if (result.ContentLength != rangeLength)
        {
            throw new Exception($"Expected {rangeLength} bytes. Was {result.ContentLength})");
        }

        byte[] buffer = ArrayPool<byte>.Shared.Rent(bufferSize); // 64KiB

        try
        {
            int read = 0;

            while ((read = await stream.ReadAsync(buffer, cancellationToken).ConfigureAwait(false)) > 0)
            {
                if (remaining is 0)
                {
                    throw new Exception("Stream returned more bytes than chuck size");
                }

                await RandomAccess.WriteAsync(fileHandle, buffer.AsMemory(0, read), fileOffset + position).ConfigureAwait(false);

                position += read;
                remaining -= read;
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }      
    }

    private static ByteRange[] GetRanges(long contentLength, int blockSize)
    {
        int blockCount = GetBlockCount(contentLength, blockSize);

        if (blockCount > 10_000)
        {
            throw new Exception("Must be less than 10,000");
        }

        var ranges = new ByteRange[blockCount];

        for (int i = 0; i < ranges.Length; i++)
        {
            int rangeSize = (i + 1) == ranges.Length // tail
                ? GetTailSize(contentLength, blockSize)
                : blockSize;

            long offset = (long)blockSize * i;

            ranges[i] = new ByteRange(
                offset,
                (offset + rangeSize) - 1
            );
        }

        return ranges;
    }

    private static int GetTailSize(long total, int blockSize)
    {
        var remaining = total % blockSize;

        return remaining is 0 ? blockSize : (int)remaining;
    }

    private static int GetBlockCount(long totalSize, int blockSize)
    {
        if (totalSize is 0) return 0;

        var blockCount = totalSize / blockSize;

        if (totalSize % blockSize != 0) blockCount++;       

        return (int)blockCount;
    }
}