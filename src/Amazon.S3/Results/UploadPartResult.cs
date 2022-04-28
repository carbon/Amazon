using Carbon.Storage;

namespace Amazon.S3;

public sealed class UploadPartResult : IUploadBlock
{
    public UploadPartResult(string uploadId, int partNumber, string eTag)
    {
        ArgumentNullException.ThrowIfNull(uploadId);
        ArgumentNullException.ThrowIfNull(eTag);

        UploadId = uploadId;
        PartNumber = partNumber;
        ETag = eTag;
    }

    public int PartNumber { get; }

    public string UploadId { get; }

    public string ETag { get; }

    // IUploadBlock

    int IUploadBlock.Number => PartNumber;

    string IUploadBlock.BlockId => ETag;

    ByteRange? IUploadBlock.Range => null;
}