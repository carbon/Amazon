using System.Xml.Serialization;

namespace Amazon.Ec2;

public sealed class DescribeVolumesResponse : IEc2Response
{
    [XmlArray("volumeSet")]
    [XmlArrayItem("item")]
    public required Volume[] Volumes { get; init; }
}