using System;

namespace Amazon.S3
{
    public class UploadPartResult : IUploadPart
    {
        public UploadPartResult(int partNumber, string uploadId, string eTag)
        {
            #region Preconditions

            if (partNumber == 0)
                throw new ArgumentException("Must not be 0", nameof(partNumber));

            if (uploadId == null)
                throw new ArgumentNullException(nameof(uploadId));

            if (eTag == null)
                throw new ArgumentNullException(nameof(ETag));

            #endregion

            PartNumber = partNumber;
            UploadId = uploadId;
            ETag = eTag;
        }

        public int PartNumber { get; }

        public string UploadId { get; }

        public string ETag { get; }

        #region Helpers

        // public byte[] MD5 => HexString.ToBytes(ETag.Trim('"'));

        #endregion
    }
}
