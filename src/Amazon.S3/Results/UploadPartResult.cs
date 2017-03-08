using System;

namespace Amazon.S3
{
    public class UploadPartResult : IUploadBlock
    {
        public UploadPartResult(string uploadId, int partNumber, string eTag)
        {
            #region Preconditions

            if (partNumber <= 0)
                throw new ArgumentException("Must be greater than 0", nameof(partNumber));

            #endregion

            UploadId = uploadId ?? throw new ArgumentNullException(nameof(uploadId));
            PartNumber = partNumber;
            ETag       = eTag ?? throw new ArgumentNullException(nameof(ETag));
        }

        public int PartNumber { get; }

        public string UploadId { get; }

        public string ETag { get; }

        #region IUploadBlock

        int IUploadBlock.Number => PartNumber;

        string IUploadBlock.ETag => ETag;

        #endregion
    }
}
