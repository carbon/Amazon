namespace Amazon.Sts;

public sealed class GetFederationTokenRequest : IStsRequest
{
    public GetFederationTokenRequest(
        string name,
        string? policy = null, 
        int? durationSeconds = null)
    {
        ArgumentNullException.ThrowIfNull(name);

        Name = name;
        Policy = policy;
        DurationSeconds = durationSeconds;
    }

    string IStsRequest.Action => "GetFederationToken";

    // 43,200 seconds (12 hours) default
    public int? DurationSeconds { get; }

    public string Name { get; }

    public string? Policy { get; }
}