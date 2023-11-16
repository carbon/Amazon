using System.Xml.Serialization;

namespace Amazon.Ec2;

public sealed class Tag
{
#nullable disable
    private Tag() { }
#nullable enable

    public Tag(string key, string value)
    {
        Key = key;
        Value = value;
    }

    [XmlElement("key")]
    public string Key { get; init; }

    [XmlElement("value")]
    public string Value { get; init; }
}