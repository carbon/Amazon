#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2;

public sealed class Vpc
{
    [XmlElement("vpcId")]
    public string VpcId { get; init; }

    [XmlElement("cidrBlock")]
    public string CidrBlock { get; init; }

    [XmlArray("ipv6CidrBlockAssociationSet")]
    [XmlArrayItem("item")]
    public Ipv6CidrBlockAssociation[] Ipv6CidrBlockAssociations { get; init; }

    // default | dedicated | host
    [XmlElement("instanceTenancy")]
    public string InstanceTenancy { get; init; }

    [XmlElement("isDefault")]
    public bool IsDefault { get; init; }

    [XmlElement("dhcpOptionsId")]
    public string DhcpOptionsId { get; init; }

    // pending | available
    [XmlElement("state")]
    public string State { get; init; }
}

/*
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
*/