using System.Diagnostics.CodeAnalysis;

namespace Amazon.Route53;

public sealed class ResourceRecord
{
    public ResourceRecord() { }

    [SetsRequiredMembers]
    public ResourceRecord(string value)
    {
        if (value.Length > 4000)
        {
            throw new ArgumentException("Must not exceed 4000 characters", nameof(value));
        }

        Value = value;
    }

    public required string Value { get; init; }
}