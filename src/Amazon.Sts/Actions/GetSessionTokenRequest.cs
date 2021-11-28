#nullable disable

namespace Amazon.Sts;

public sealed class GetSessionTokenRequest : IStsRequest
{
    public string Action => "GetSessionToken";

    public int DurationInSeconds { get; init; }

    public string SerialNumber { get; init; }

    public string TokenCode { get; init; }
}