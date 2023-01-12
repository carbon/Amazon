namespace Amazon.Sts;

public sealed class GetSessionTokenRequest : IStsRequest
{
    string IStsRequest.Action => "GetSessionToken";

    public int? DurationInSeconds { get; init; }

    public string? SerialNumber { get; init; }

    public string? TokenCode { get; init; }
}