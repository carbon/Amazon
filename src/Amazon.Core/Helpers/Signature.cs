using System;
using System.Security.Cryptography;
using System.Text;

using Amazon.Helpers;

namespace Amazon
{
    internal readonly struct Signature
    {
        public Signature(byte[] data)
        {
            Data = data;
        }

        public byte[] Data { get; }

        public readonly string ToBase64String() => Convert.ToBase64String(Data);

        public readonly string ToHexString() => HexString.FromBytes(Data);

        public static Signature ComputeHmacSha256(string key, string data)
        {
            return ComputeHmacSha256(
                key  : Encoding.UTF8.GetBytes(key), 
                data : Encoding.UTF8.GetBytes(data)
           );
        }

        public static Signature ComputeHmacSha256(byte[] key, byte[] data)
        {
            using HMACSHA256 algorithm = new HMACSHA256(key);

            byte[] hash = algorithm.ComputeHash(data);

            return new Signature(hash);
        }
    }
}