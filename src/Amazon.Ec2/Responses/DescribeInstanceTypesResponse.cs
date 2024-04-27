using System.Xml.Serialization;

namespace Amazon.Ec2;

[XmlRoot("DescribeInstanceTypesResponse", Namespace = Ec2Client.Namespace)]
public sealed class DescribeInstanceTypesResponse
{
    [XmlElement("requestId")]
    public required string RequestId { get; init; }

    [XmlElement("nextToken")]
    public string? NextToken { get; init; }

    [XmlArray("instanceTypeSet")]
    [XmlArrayItem("item")]
    public required InstanceTypeInfo[] InstanceTypes { get; init; }

    public static DescribeInstanceTypesResponse Deserialize(string text)
    {
        return Ec2Serializer<DescribeInstanceTypesResponse>.Deserialize(text);
    }
}
