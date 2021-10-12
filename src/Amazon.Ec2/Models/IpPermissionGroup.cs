#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2;

public sealed class IpPermissionGroup
{
    [XmlElement("userId")]
    public string UserId { get; init; }

    [XmlElement("groupId")]
    public string GroupId { get; init; }

    [XmlElement("vpcId")]
    public string VpcId { get; init; }

    [XmlElement("vpcPeeringConnectionId")]
    public string VpcPeeringConnectionId { get; init; }

    [XmlElement("peeringStatus")]
    public string PeeringStatus { get; init; }
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
       