using System;

namespace Amazon.S3
{
    // PUT /ObjectName?partNumber=PartNumber&uploadId=UploadId
    public class UploadPartRequest : PutObjectRequest
    {
        public UploadPartRequest(AwsRegion region, string bucketName, string key, int partNumber, string uploadId)
            : base(region, bucketName, key + $"?partNumber={partNumber}&uploadId={uploadId}")
        {
            #region Preconditions

            if (partNumber < 1 || partNumber > 10000)
                throw new ArgumentOutOfRangeException("partNumber", partNumber, "Must be between 1 and 10,000");

            if (uploadId == null)
                throw new ArgumentNullException(nameof(uploadId));

            #endregion

            PartNumber = partNumber;
            UploadId = uploadId;
        }

        public int PartNumber { get; }

        public string UploadId { get; }
    }
}

/*
Part numbers:	1 to 10,000 (inclusive)
Part size:		5 MB to 5 GB, last part can be < 5 MB
*/
