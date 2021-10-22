#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb;

public sealed class TargetDescription
{
    public TargetDescription() { }

    public TargetDescription(string id, int? port = null)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
        Port = port;
    }

    [Required]
    public string Id { get; init; }

    [Range(1, 65535)]
    public int? Port { get; init; }
}