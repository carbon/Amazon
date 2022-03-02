using System.IO;
using System.Threading;

namespace Amazon.S3.Extensions;

public static class S3BucketExtensions
{
    public static async Task DownloadAsync(
       this S3Bucket bucket,
       string objectKey,
       string destinationPath,
       S3DownloadOptions? options = null,
       CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(objectKey);
        ArgumentNullException.ThrowIfNull(destinationPath);

        options ??= S3DownloadOptions.Default;

        if (options.MaxThreadCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(options.MaxThreadCount), options.MaxThreadCount, "Must be > 0");
        }

        if (options.MaxThreadCount > 1)
        {
            await bucket.Client.DownloadAsync(bucket.Name, objectKey, destinationPath, options, cancellationToken);
        }
        else
        {
            using var blob = (S3Object)(await bucket.GetAsync(objectKey, cancellationToken).ConfigureAwait(false));

            using var fileStream = new FileStream(destinationPath, new FileStreamOptions {
                Access            = FileAccess.Write,
                Mode              = FileMode.CreateNew,
                Options           = FileOptions.Asynchronous | FileOptions.WriteThrough,
                PreallocationSize = blob.ContentLength
            });

            await blob.CopyToAsync(fileStream, cancellationToken).ConfigureAwait(false);
        }
    }
}
