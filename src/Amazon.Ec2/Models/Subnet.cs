#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public sealed class Subnet
    {
        [XmlElement("subnetId")]
        public string SubnetId { get; init; }

        [XmlElement("state")]
        public string State { get; init; }

        [XmlElement("vpcId")]
        public string VpcId { get; init; }

        [XmlElement("cidrBlock")]
        public string CidrBlock { get; init; }

        [XmlArray("ipv6CidrBlockAssociationSet")]
        [XmlArrayItem("item")]
        public Ipv6CidrBlockAssociation[] Ipv6CidrBlockAssociations { get; init; }

        [XmlElement("availableIpAddressCount")]
        public int AvailableIpAddressCount { get; init; }
        
        [XmlElement("availabilityZone")]
        public string AvailabilityZone { get; init; }

        [XmlElement("defaultForAz")]
        public bool DefaultForAz { get; init; }

        [XmlElement("mapPublicIpOnLaunch")]
        public bool MapPublicIpOnLaunch { get; init; }

        [XmlElement("assignIpv6AddressOnCreation")]
        public bool AssignIpv6AddressOnCreation { get; init; }
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