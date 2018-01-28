using System;
using System.Security.Cryptography;
using System.Text;

namespace Amazon
{
    using Helpers;

    internal readonly struct Signature
    {
        public Signature(byte[] data)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public byte[] Data { get; }

        public string ToBase64String() => Convert.ToBase64String(Data);

        public string ToHexString() => HexString.FromBytes(Data);

        public static Signature ComputeHmacSha256(string key, string data)
        {
            return ComputeHmacSha256(
                key  : Encoding.UTF8.GetBytes(key), 
                data : Encoding.UTF8.GetBytes(data)
           );
        }

        public static Signature ComputeHmacSha256(byte[] key, byte[] data)
        {
            #region Preconditions

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (data == null)
                throw new ArgumentNullException(nameof(data));

            #endregion

            using (var algorithm = new HMACSHA256(key))
            {
                byte[] hash = algorithm.ComputeHash(data);

                return new Signature(hash);
            }
        }
    }
}