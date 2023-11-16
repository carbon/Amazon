namespace Amazon.Metadata;

internal sealed class MetadataToken(string value, DateTime expires)
{
    public string Value { get; } = value ?? throw new ArgumentNullException(nameof(value));

    public DateTime Expires { get; } = expires;
}