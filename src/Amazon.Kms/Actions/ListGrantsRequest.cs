#nullable disable

namespace Amazon.Kms
{
    public sealed class ListGrantsRequest : KmsRequest
    {
        public string KeyId { get; set; }

        public int Limit { get; set; }

        public string Marker { get; set; }
    }
}