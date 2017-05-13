using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public class DescribeVpcsResponse : IEc2Response
    {
        [XmlArray("vpcSet")]
        [XmlArrayItem("item")]
        public Vpc[] Vpcs { get; set; }
    }
}