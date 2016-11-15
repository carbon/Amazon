namespace Amazon.S3
{
    public interface IUploadPart
    {
        int PartNumber { get; }

        string UploadId { get; }

        string ETag { get; }
    }
}
