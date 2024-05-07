namespace Amazon.Route53;

public sealed class GetChangeRequest(string id)
{
    // Max Length = 32
    public string Id { get; } = id;
}