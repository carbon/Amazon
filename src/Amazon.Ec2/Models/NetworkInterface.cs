using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public class NetworkInterface
    {
        [XmlElement("networkInterfaceId")]
        public string NetworkInterfaceId { get; set; }

        [XmlElement("subnetId")]
        public string SubnetId { get; set; }

        [XmlElement("vpcId")]
        public string VpcId { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("ownerId")]
        public string OwnerId { get; set; }

        [XmlElement("status")]
        public string Status { get; set; }

        [XmlElement("macAddress")]
        public string MacAddress { get; set; }

        [XmlElement("privateIpAddress")]
        public string PrivateDnsName { get; set; }

        [XmlElement("sourceDestCheck")]
        public string SourceDestCheck { get; set; }

        [XmlArray("groupSet")]
        [XmlArrayItem("item")]
        public List<NetworkInterfaceSecurityGroup> Groups { get; set; }

        [XmlElement("attachment")]
        public NetworkInterfaceAttachment Attachment { get; set; }

        private static readonly XmlSerializer serializer = new XmlSerializer(
          typeof(NetworkInterface),
          new XmlRootAttribute {
              ElementName = "item",
              Namespace = Ec2Client.Namespace
          }
        );

        public static NetworkInterface Deserialize(XElement element)
        {
            return (NetworkInterface)serializer.Deserialize(element.CreateReader());
        }
    }
}

/*
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

*/