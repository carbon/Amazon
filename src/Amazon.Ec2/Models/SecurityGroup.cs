using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public class SecurityGroup
    {
        [XmlElement("ownerId")]
        public long OwnerId { get; set; }
        
        [XmlElement("groupId")]
        public string GroupId { get; set; }

        [XmlElement("groupName")]
        public string GroupName { get; set; }

        [XmlElement("groupDescription")]
        public string GroupDescription { get; set; }

        [XmlElement("vpcId")]
        public string VpcId { get; set; }

        [XmlArray("ipPermissions")]
        [XmlArrayItem("item")]
        public IpPermission[] IpPermissions { get; set; }

        [XmlArray("ipPermissionsEgress")]
        [XmlArrayItem("item")]
        public IpPermission[] IpPermissionsEgress { get; set; }
    }

    public class IpPermissionGroup
    {
        [XmlElement("userId")]
        public string UserId { get; set; }

        [XmlElement("groupId")]
        public string GroupId { get; set; }

        [XmlElement("vpcId")]
        public string VpcId { get; set; }

        [XmlElement("vpcPeeringConnectionId")]
        public string VpcPeeringConnectionId { get; set; }

        [XmlElement("peeringStatus")]
        public string PeeringStatus { get; set; }
    }
    
    public class IpRange
    {
        [XmlElement("cidrIp")]
        public string CidrIp { get; set; }
    }

    public class Ipv6Range
    {
        [XmlElement("cidrIpv6")]
        public string CidrIpv6 { get; set; }
    }
}

/*
<item>
    <ownerId>123456789012</ownerId>
    <groupId>sg-1a2b3c4d</groupId>
    <groupName>WebServers</groupName>
    <groupDescription>Web Servers</groupDescription>
    <vpcId>vpc-614cc409</vpcId>
    <ipPermissions>
        <item>
            <ipProtocol>-1</ipProtocol>
            <groups>
                <item>
                    <userId>123456789012</userId>
                    <groupId>sg-af8661c0</groupId>
                </item>
            </groups>
            <ipRanges/>
            <prefixListIds/>
        </item>
        <item>
            <ipProtocol>tcp</ipProtocol>
            <fromPort>22</fromPort>
            <toPort>22</toPort>
            <groups/>
            <ipRanges>
                <item>
                    <cidrIp>204.246.162.38/32</cidrIp>
                </item>
            </ipRanges>
            <prefixListIds/>
        </item>
    </ipPermissions>
    <ipPermissionsEgress>
        <item>
            <ipProtocol>-1</ipProtocol>
            <groups/>
            <ipRanges>
                <item>
                    <cidrIp>0.0.0.0/0</cidrIp>
                </item>
            </ipRanges>
            <prefixListIds/>
        </item>
    </ipPermissionsEgress>
</item>
*/
       