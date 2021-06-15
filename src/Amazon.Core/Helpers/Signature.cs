using System;
using System.Security.Cryptography;

namespace Amazon
{
    internal readonly struct Signature
    {
        public Signature(byte[] data)
        {
            Data = data;
        }

        public byte[] Data { get; }

        public static Signature ComputeHmacSha256(byte[] key, byte[] data)
        {
            using var algorithm = new HMACSHA256(key);

            byte[] hash = algorithm.ComputeHash(data);

            return new Signature(hash);
        }
    }
}