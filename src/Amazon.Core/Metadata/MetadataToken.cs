using System;

namespace Amazon.Metadata;

internal sealed class MetadataToken
{
    public MetadataToken(string value, DateTime expires)
    {
        Value = value;
        Expires = expires;
    }

    public string Value { get; }

    public DateTime Expires { get; }
}