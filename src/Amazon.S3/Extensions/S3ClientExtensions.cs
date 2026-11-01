using System.Buffers;
using System.IO;
using System.Net.Http;
using System.Threading;

using Carbon.Storage;
using Carbon.Storage.Helpers;

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

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(options.MaxThreadCount);

        var metadata = await s3Client.GetObjectHeadAsync(new ObjectHeadRequest(s3Client.Host, bucketName, objectKey), cancellationToken);

        long total = metadata.ContentLength;

        var ranges = GetRanges(total, options.ChunkSize);

        using var fileHandle = File.OpenHandle(
            path              : destinationPath,
            mode              : FileMode.CreateNew,
            access            : FileAccess.ReadWrite, FileShare.None, 
            options           : FileOptions.WriteThrough | FileOptions.Asynchronous, 
            preallocationSize : total
        );

        var parallelOptions = new ParallelOptions {
            MaxDegreeOfParallelism = options.MaxThreadCount,
            CancellationToken      = cancellationToken
        };

        // Stops on the first failure, and waits for in-flight chunks before the file handle is closed
        await Parallel.ForEachAsync(ranges, parallelOptions, async (range, token) => {
            await s3Client.DownloadByteRange(bucketName, objectKey, range, fileHandle, token).ConfigureAwait(false);
        }).ConfigureAwait(false);

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

        byte[] rentedBuffer = ArrayPool<byte>.Shared.Rent(bufferSize); // 64KiB

        try
        {
            int read = 0;

            while ((read = await stream.ReadAsync(rentedBuffer, cancellationToken).ConfigureAwait(false)) > 0)
            {
                if (remaining is 0)
                {
                    throw new Exception("Stream returned more bytes than chuck size");
                }

                await RandomAccess.WriteAsync(fileHandle, rentedBuffer.AsMemory(0, read), fileOffset + position, cancellationToken).ConfigureAwait(false);

                position += read;
                remaining -= read;
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(rentedBuffer);
        }      
    }

    private static ByteRange[] GetRanges(long contentLength, int blockSize)
    {
        int blockCount = BlockHelper.GetBlockCount(contentLength, blockSize);

        if (blockCount > 10_000)
        {
            throw new Exception("Must be less than 10,000");
        }

        var ranges = new ByteRange[blockCount];

        for (int i = 0; i < ranges.Length; i++)
        {
            int rangeSize = (i + 1) == ranges.Length // tail
                ? BlockHelper.GetTailSize(contentLength, blockSize)
                : blockSize;

            long offset = (long)blockSize * i;

            ranges[i] = new ByteRange(
                offset,
                (offset + rangeSize) - 1
            );
        }

        return ranges;
    }
}