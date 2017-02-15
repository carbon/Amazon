using System;

namespace Amazon.S3
{
    public struct ETag
    {
        public ETag(string value)
        {
            Value = value;
        }

        public byte[] AsMD5()
        {
            if (Value == null || Value.Contains("-")) return null;

            // Generally the ETAG is the MD5 of the object -- hexidecimal encoded and wrapped in quootes.
            // If the object was uploaded using multipart upload then this is the MD5 all of the upload-part-md5s.

            // Multipart uploads also contain a dash

            // 1f8ada2ce841b291cfcd6b9b4b645044-2

            return HexStringToBytes(Value.Trim('"'));
        }

        public string Value { get; }

        public static implicit operator string(ETag tag) => tag.Value;

        #region Private Helpers

        private static byte[] HexStringToBytes(string hexString)
        {
            #region Preconditions

            if (hexString == null)
                throw new ArgumentNullException(nameof(hexString));

            if (hexString.Length % 2 != 0)
                throw new ArgumentException("Must be divisible by 2");

            #endregion

            byte[] bytes = new byte[hexString.Length / 2];

            for (int i = 0; i < hexString.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }

            return bytes;
        }

        #endregion
    }
}
