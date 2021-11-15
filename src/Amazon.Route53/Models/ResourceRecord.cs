namespace Amazon.Route53;

public sealed class ResourceRecord
{
#nullable disable
    public ResourceRecord() { }
#nullable enable

    public ResourceRecord(string value)
    {
        Value = value;
    }

    public string Value { get; init; }
}
