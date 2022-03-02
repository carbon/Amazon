namespace Amazon.S3.Extensions;

public sealed class S3DownloadOptions
{
    public static readonly S3DownloadOptions Default = new ();

    public int ChunkSize { get; init; } = 1024 * 1024 * 16; // 16MiB;

    public int MaxThreadCount { get; init; } = 1;
}