namespace Amazon.Route53;

public sealed class ChangeInfo
{
    public required string Id { get; init; }

    public required string Status { get; init; }

    public required DateTime SubmittedAt { get; init; }

    public string? Comment { get; init; }
}