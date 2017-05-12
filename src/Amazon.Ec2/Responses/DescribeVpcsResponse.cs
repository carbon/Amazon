using System.Collections.Generic;
using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public class DescribeVpcsResponse : IEc2Response
    {
        [XmlArray("vpcSet")]
        [XmlArrayItem("item")]
        public List<Vpc> Vpcs { get; } = new List<Vpc>();
    }
}