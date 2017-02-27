namespace Amazon.S3
{
    public interface IUploadBlock
    {
        string UploadId { get; }

        string ETag { get; }

        int Number { get; }
    }
}