using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Amazon.Elb;

public sealed class TargetDescription
{
    public TargetDescription() { }

    [SetsRequiredMembers]
    public TargetDescription(string id, int? port = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);

        Id = id;
        Port = port;
    }

    public required string Id { get; init; }

    [Range(1, 65535)]
    public int? Port { get; init; }
}