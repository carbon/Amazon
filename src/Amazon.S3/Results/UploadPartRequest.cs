using System;

using Carbon.Storage;

namespace Amazon.S3
{
    // PUT /ObjectName?partNumber=PartNumber&uploadId=UploadId
    public class UploadPartRequest : PutObjectRequest
    {
        public UploadPartRequest(AwsRegion region, IUpload upload, int partNumber)
            : this(region, upload.BucketName, upload.ObjectName, upload.UploadId, partNumber) { }

        public UploadPartRequest(AwsRegion region, string bucketName, string key, string uploadId, int partNumber)
            : base(region, bucketName, key + $"?partNumber={partNumber}&uploadId={uploadId}")
        {
            #region Preconditions

            if (partNumber < 1 || partNumber > 10000)
                throw new ArgumentOutOfRangeException(nameof(partNumber), partNumber, "Must be between 1 and 10,000");

            #endregion

            UploadId   = uploadId ?? throw new ArgumentNullException(nameof(uploadId));
            PartNumber = partNumber;
        }

        public string UploadId { get; }

        public int PartNumber { get; }
    }
}

/*
Part numbers:	1 to 10,000 (inclusive)
Part size:		5 MB to 5 GB, last part can be < 5 MB
*/
