#nullable disable

using System.Xml.Linq;
using System.Xml.Serialization;

namespace Amazon.Ec2;

public sealed class Instance
{
    [XmlElement("architechure")]
    public string Architecture { get; init; } // i386 | x86_64

    [XmlElement("hypervisor")]
    public string Hypervisor { get; init; } // ovm | xen

    [XmlElement("imageId")]
    public string ImageId { get; init; }

    [XmlElement("instanceId")]
    public string InstanceId { get; init; }

    // [XmlElement("instanceLifecycle")]
    // public string InstanceLifecycle { get; set; }

    [XmlElement("instanceState")]
    public InstanceState InstanceState { get; init; }

    [XmlElement("instanceType")]
    public string InstanceType { get; init; }

    [XmlElement("placement")]
    public Placement Placement { get; init; }

    [XmlArray("networkInterfaceSet")]
    [XmlArrayItem("item")]
    public NetworkInterface[] NetworkInterfaces { get; init; }

    [XmlArray("blockDeviceMapping")]
    [XmlArrayItem("item")]
    public BlockDeviceMapping[] BlockDeviceMappings { get; init; }


#nullable enable

    /// <summary>
    /// The public IP address assigned to the instance, if applicable.
    /// </summary>
    [XmlElement("ipAddress")]
    public string? IpAddress { get; init; }

#nullable disable

    [XmlElement("kernelId")]
    public string KernelId { get; init; }

    [XmlElement("launchTime")]
    public DateTime LaunchTime { get; init; }

    [XmlElement("platform")]
    public string Platform { get; init; }

    [XmlElement("privateIpAddress")]
    public string PrivateIpAddress { get; init; }

    [XmlElement("rootDeviceName")]
    public string RootDeviceName { get; init; }

    [XmlElement("vpcId")]
    public string VpcId { get; init; }

    private static readonly XmlSerializer serializer = new(
        typeof(Instance),
        new XmlRootAttribute
        {
            ElementName = "item",
            Namespace = Ec2Client.Namespace
        }
    );

    public static Instance Deserialize(XElement element)
    {
        return (Instance)serializer.Deserialize(element.CreateReader());
    }
}


/*
<item>
    <instanceId>i-1234567890abcdef0</instanceId>
    <imageId>ami-bff32ccc</imageId>
    <instanceState>
        <code>16</code>
        <name>running</name>
    </instanceState>
    <privateDnsName>ip-192-168-1-88.eu-west-1.compute.internal</privateDnsName>
    <dnsName>ec2-54-194-252-215.eu-west-1.compute.amazonaws.com</dnsName>
    <reason/>
    <keyName>my_keypair</keyName>
    <amiLaunchIndex>0</amiLaunchIndex>
    <productCodes/>
    <instanceType>t2.micro</instanceType>
    <launchTime>2015-12-22T10:44:05.000Z</launchTime>
    <placement>
        <availabilityZone>eu-west-1c</availabilityZone>
        <groupName/>
        <tenancy>default</tenancy>
    </placement>
    <monitoring>
        <state>disabled</state>
    </monitoring>
    <subnetId>subnet-56f5f633</subnetId>
    <vpcId>vpc-11112222</vpcId>
    <privateIpAddress>192.168.1.88</privateIpAddress>
    <ipAddress>54.194.252.215</ipAddress>
    <sourceDestCheck>true</sourceDestCheck>
    <groupSet>
        <item>
            <groupId>sg-e4076980</groupId>
            <groupName>SecurityGroup1</groupName>
        </item>
    </groupSet>
    <architecture>x86_64</architecture>
    <rootDeviceType>ebs</rootDeviceType>
    <rootDeviceName>/dev/xvda</rootDeviceName>
    <blockDeviceMapping>
        <item>
            <deviceName>/dev/xvda</deviceName>
            <ebs>
                <volumeId>vol-1234567890abcdef0</volumeId>
                <status>attached</status>
                <attachTime>2015-12-22T10:44:09.000Z</attachTime>
                <deleteOnTermination>true</deleteOnTermination>
            </ebs>
        </item>
    </blockDeviceMapping>
    <virtualizationType>hvm</virtualizationType>
    <clientToken>xMcwG14507example</clientToken>
    <tagSet>
        <item>
            <key>Name</key>
            <value>Server_1</value>
        </item>
    </tagSet>
    <hypervisor>xen</hypervisor>
    <networkInterfaceSet>
        <item>
            <networkInterfaceId>eni-551ba033</networkInterfaceId>
            <subnetId>subnet-56f5f633</subnetId>
            <vpcId>vpc-11112222</vpcId>
            <description>Primary network interface</description>
            <ownerId>123456789012</ownerId>
            <status>in-use</status>
            <macAddress>02:dd:2c:5e:01:69</macAddress>
            <privateIpAddress>192.168.1.88</privateIpAddress>
            <privateDnsName>ip-192-168-1-88.eu-west-1.compute.internal</privateDnsName>
            <sourceDestCheck>true</sourceDestCheck>
            <groupSet>
                <item>
                    <groupId>sg-e4076980</groupId>
                    <groupName>SecurityGroup1</groupName>
                </item>
            </groupSet>
            <attachment>
                <attachmentId>eni-attach-39697adc</attachmentId>
                <deviceIndex>0</deviceIndex>
                <status>attached</status>
                <attachTime>2015-12-22T10:44:05.000Z</attachTime>
                <deleteOnTermination>true</deleteOnTermination>
            </attachment>
            <association>
                <publicIp>54.194.252.215</publicIp>
                <publicDnsName>ec2-54-194-252-215.eu-west-1.compute.amazonaws.com</publicDnsName>
                <ipOwnerId>amazon</ipOwnerId>
            </association>
            <privateIpAddressesSet>
                <item>
                    <privateIpAddress>192.168.1.88</privateIpAddress>
                    <privateDnsName>ip-192-168-1-88.eu-west-1.compute.internal</privateDnsName>
                    <primary>true</primary>
                    <association>
                    <publicIp>54.194.252.215</publicIp>
                    <publicDnsName>ec2-54-194-252-215.eu-west-1.compute.amazonaws.com</publicDnsName>
                    <ipOwnerId>amazon</ipOwnerId>
                    </association>
                </item>
            </privateIpAddressesSet>
        </item>
    </networkInterfaceSet>
    <ebsOptimized>false</ebsOptimized>
</item>
*/