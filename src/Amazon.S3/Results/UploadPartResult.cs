using System;

namespace Amazon.S3
{
    public class UploadPartResult : IUploadBlock
    {
        public UploadPartResult(string uploadId, int partNumber, string eTag)
        {
            #region Preconditions

            if (partNumber == 0)
                throw new ArgumentException("Must not be 0", nameof(partNumber));

            if (uploadId == null)
                throw new ArgumentNullException(nameof(uploadId));

            if (eTag == null)
                throw new ArgumentNullException(nameof(ETag));

            #endregion

            UploadId   = uploadId;
            PartNumber = partNumber;
            ETag       = eTag;
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
