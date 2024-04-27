using System.Xml.Serialization;

namespace Amazon.Ec2;

public sealed class DescribeImagesResponse : IEc2Response
{
    [XmlArray("imagesSet")]
    [XmlArrayItem("item")]
    public required Image[] Images { get; init; }

    [XmlElement("nextToken")]
    public string? NextToken { get; init; }
}