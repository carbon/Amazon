#nullable disable

namespace Amazon.Route53;

public sealed class ChangeInfo
{
    public string Comment { get; init; }

    public string Id { get; init; }

    public string Status { get; init; }

    public DateTime SubmittedAt { get; init; }
}