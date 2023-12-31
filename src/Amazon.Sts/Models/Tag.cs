using System.ComponentModel.DataAnnotations;

namespace Amazon.Sts;

public sealed class Tag
{
    [StringLength(128)]
    public required string Key { get; init; }

    [StringLength(256)]
    public required string Value { get; init; }
}