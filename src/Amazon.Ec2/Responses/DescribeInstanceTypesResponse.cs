#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2
{
    [XmlRoot("DescribeInstanceTypesResponse", Namespace = Ec2Client.Namespace)]
    public sealed class DescribeInstanceTypesResponse
    {
        [XmlElement("requestId")]
        public string RequestId { get; set; }

        [XmlElement("nextToken")]
        public string NextToken { get; set; }

        [XmlArray("instanceTypeSet")]
        [XmlArrayItem("item")]
        public InstanceTypeInfo[] InstanceTypes { get; set; }

        public static DescribeInstanceTypesResponse Deserialize(string text)
        {
            return Ec2Serializer<DescribeInstanceTypesResponse>.Deserialize(text);
        }
    }
}