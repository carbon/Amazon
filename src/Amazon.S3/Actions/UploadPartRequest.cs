namespace Amazon.S3;

// PUT /ObjectName?partNumber=PartNumber&uploadId=UploadId
public sealed class UploadPartRequest : PutObjectRequest
{
    public UploadPartRequest(string host, string bucketName, string key, string uploadId, int partNumber)
        : base(host, bucketName, key + $"?partNumber={partNumber}&uploadId={uploadId}")
    {
        if (partNumber < 1 || partNumber > 10_000)
        {
            throw new ArgumentOutOfRangeException(nameof(partNumber), partNumber, "Must be between 1 and 10,000");
        }

        UploadId = uploadId ?? throw new ArgumentNullException(nameof(uploadId));
        PartNumber = partNumber;
    }

    public string UploadId { get; }

    public int PartNumber { get; }
}

/*
Part numbers:	1 to 10,000 (inclusive)
Part size:		5 MB to 5 GB, last part can be < 5 MB
*/
