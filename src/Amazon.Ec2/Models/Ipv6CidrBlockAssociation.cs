#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public class Ipv6CidrBlockAssociation
    {
        [XmlElement("associationId")]
        public string AssociationId { get; init; }

        [XmlElement("ipv6CidrBlock")]
        public string Ipv6CidrBlock { get; init; }

        [XmlElement("ipv6CidrBlockState")]
        public Ipv6CidrBlockState Ipv6CidrBlockState { get; init; }
    }
}

/*
<item>
    <ipv6CidrBlock>2001:db8:1234:1a00::/64</ipv6CidrBlock>
    <associationId>subnet-cidr-assoc-abababab</associationId>
    <ipv6CidrBlockState>
        <state>ASSOCIATED</state>
    </ipv6CidrBlockState>
</item>
*/