using System;

namespace Amazon.S3
{
    internal readonly struct ServerSideEncryptionKey
    {
        public ServerSideEncryptionKey(byte[] key, string algorithm = "AES256")
        {
            if (key.Length != 32)
            {
                throw new ArgumentException("Must be 256 bits", nameof(key));
            }

            Algorithm = algorithm;
            Key = key;
        }

        public string Algorithm { get; }

        public byte[] Key { get; }

        public readonly byte[] KeyMD5 => HashHelper.ComputeMD5Hash(Key);
    }
}