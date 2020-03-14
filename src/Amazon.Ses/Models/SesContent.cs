using System;
using System.Runtime.Serialization;

namespace Amazon.Ses
{
    public sealed class SesContent
    {
        public SesContent(string data, CharsetType charset = CharsetType.SevenBitASCII)
        {
            Charset = charset == CharsetType.UTF8 ? "UTF-8" : null;
            Data    = data ?? throw new ArgumentNullException(nameof(data));            
        }

        public string? Charset { get; }

        public string Data { get; }
    }

    public enum CharsetType : byte
    {
        SevenBitASCII = 1,
        UTF8 = 2
    }
}