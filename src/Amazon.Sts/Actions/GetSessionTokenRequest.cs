#nullable disable

namespace Amazon.Sts
{
    public sealed class GetSessionTokenRequest : IStsRequest
    {
        public string Action => "GetSessionToken";

        public int DurationInSeconds { get; set; }

        public string SerialNumber { get; set; }

        public string TokenCode { get; set; }
    }
}