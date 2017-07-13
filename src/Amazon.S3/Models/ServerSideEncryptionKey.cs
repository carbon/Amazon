using System;
using System.Security.Cryptography;

namespace Amazon.S3
{
    internal struct ServerSideEncryptionKey
    {
        public ServerSideEncryptionKey(byte[] key)
        {
            #region Preconditions

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (key.Length != 32)
                throw new ArgumentException("Must be 256 bits", nameof(key));

            #endregion

            Key = key;
        }

        public string Algorithm => "AES256";

        public byte[] Key { get; }

        public byte[] KeyMD5
        {
            get
            {
                using (var md5 = MD5.Create())
                {
                    return md5.ComputeHash(Key);
                }
            }
        }
    }
}