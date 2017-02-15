using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Amazon.Ec2
{
    [XmlRoot("DescribeVpcsResponse", Namespace = "http://ec2.amazonaws.com/doc/2016-09-15/")]
    public class DescribeVpcsResponse
    {
        [XmlArray("vpcSet")]
        [XmlArrayItem("item")]
        public List<Vpc> Vpcs { get; } = new List<Vpc>();

        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(DescribeVpcsResponse));

        public static DescribeVpcsResponse Parse(string text)
        {
            using (var reader = new StringReader(text))
            {
                return (DescribeVpcsResponse)serializer.Deserialize(reader);
            }          
        }
    }
}

/*
<DescribeVpcsResponse xmlns="http://ec2.amazonaws.com/doc/2016-11-15/">
  <requestId>7a62c49f-347e-4fc4-9331-6e8eEXAMPLE</requestId>
  <vpcSet>
    <item>
      <vpcId>vpc-1a2b3c4d</vpcId>
      <state>available</state>
      <cidrBlock>10.0.0.0/23</cidrBlock>
      <ipv6CidrBlockAssociationSet>
        <item>
          <ipv6CidrBlock>2001:db8:1234:1a00::/56</ipv6CidrBlock>
          <associationId>vpc-cidr-assoc-abababab</associationId>
          <ipv6CidrBlockState>
             <state>ASSOCIATED</state>
          </ipv6CidrBlockState>
        </item>
     </ipv6CidrBlockAssociationSet>    
      <dhcpOptionsId>dopt-7a8b9c2d</dhcpOptionsId> 
      <instanceTenancy>default</instanceTenancy>
      <isDefault>false</isDefault>
      <tagSet/>
    </item>
  </vpcSet>
</DescribeVpcsResponse>
*/
