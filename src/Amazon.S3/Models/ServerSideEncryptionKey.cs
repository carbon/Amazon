using System;
using System.Security.Cryptography;

namespace Amazon.S3
{
    internal readonly struct ServerSideEncryptionKey
    {
        public ServerSideEncryptionKey(byte[] key, string algorithm = "AES256")
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));

            if (key.Length != 32)
                throw new ArgumentException("Must be 256 bits", nameof(key));

            Algorithm = algorithm ?? throw new ArgumentNullException(nameof(algorithm));
            Key = key;
        }

        public string Algorithm { get; }

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