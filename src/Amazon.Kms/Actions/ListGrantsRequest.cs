#nullable disable

using System;

namespace Amazon.Kms
{
    public sealed class ListGrantsRequest : KmsRequest
    {
        public ListGrantsRequest() { }

        public ListGrantsRequest(string keyId)
        {
            KeyId = keyId ?? throw new ArgumentNullException(nameof(keyId));
        }

        public string KeyId { get; set; }

        public int Limit { get; set; }

#nullable enable

        public string? Marker { get; set; }
    }
}