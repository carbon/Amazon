#nullable disable

using System.Runtime.Serialization;

namespace Amazon.Ec2;

public sealed class TagSpecification
{
    public TagSpecification() { }

    public TagSpecification(string resourceType, params Tag[] tags)
    {
        ResourceType = resourceType;
        Tags = tags;
    }

    [DataMember(Name = "Tag")]
    public Tag[] Tags { get; init; }

    // instance and volume.
    public string ResourceType { get; init; }
}
