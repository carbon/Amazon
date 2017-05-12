using System.Xml.Linq;
using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public class Subnet
    {
        [XmlElement("subnetId")]
        public string SubnetId { get; set; }

        [XmlElement("state")]
        public string State { get; set; }

        [XmlElement("vpcId")]
        public string VpcId { get; set; }

        [XmlElement("cidrBlock")]
        public string CidrBlock { get; set; }

        [XmlElement("availableIpAddressCount")]
        public int AvailableIpAddressCount { get; set; }
        
        [XmlElement("availabilityZone")]
        public string AvailabilityZone { get; set; }

        [XmlElement("defaultForAz")]
        public bool DefaultForAz { get; set; }

        [XmlElement("mapPublicIpOnLaunch")]
        public bool MapPublicIpOnLaunch { get; set; }

        [XmlElement("assignIpv6AddressOnCreation")]
        public bool AssignIpv6AddressOnCreation { get; set; }

        private static readonly XmlSerializer serializer = new XmlSerializer(
           typeof(Subnet),
           new XmlRootAttribute
           {
               ElementName = "item",
               Namespace = Ec2Client.Namespace
           }
       );

        public static Subnet Deserialize(XElement element)
        {
            return (Subnet)serializer.Deserialize(element.CreateReader());
        }
    }
}

/*
<item>
    <subnetId>subnet-9d4a7b6c</subnetId>
    <state>available</state>
    <vpcId>vpc-1a2b3c4d</vpcId>
    <cidrBlock>10.0.1.0/24</cidrBlock>
    <ipv6CidrBlockAssociationSet>
    <item>
        <ipv6CidrBlock>2001:db8:1234:1a00::/64</ipv6CidrBlock>
        <associationId>subnet-cidr-assoc-abababab</associationId>
        <ipv6CidrBlockState>
            <state>ASSOCIATED</state>
        </ipv6CidrBlockState>
        </item>
    </ipv6CidrBlockAssociationSet>
    <availableIpAddressCount>251</availableIpAddressCount>
    <availabilityZone>us-east-1a</availabilityZone>
    <defaultForAz>false</defaultForAz>
    <mapPublicIpOnLaunch>false</mapPublicIpOnLaunch>
    <tagSet/>
    <assignIpv6AddressOnCreation>false</assignIpv6AddressOnCreation>
</item>
<item>
    <subnetId>subnet-6e7f829e</subnetId>
    <state>available</state>
    <vpcId>vpc-1a2b3c4d></vpcId>
    <cidrBlock>10.0.0.0/24</cidrBlock>
    <ipv6CidrBlockAssociationSet/> 
    <availableIpAddressCount>251</availableIpAddressCount>
    <availabilityZone>us-east-1a</availabilityZone>
    <defaultForAz>false</defaultForAz>
    <mapPublicIpOnLaunch>false</mapPublicIpOnLaunch>
    <assignIpv6AddressOnCreation>false</assignIpv6AddressOnCreation>
</item>
*/