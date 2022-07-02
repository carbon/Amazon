namespace Amazon.Ec2.Tests;

public class DescribeNetworkInterfacesTests
{
    [Fact]
    public void CanDeserialize()
    {
        var response = Ec2Serializer<DescribeNetworkInterfacesResponse>.Deserialize(
            """
            <DescribeNetworkInterfacesResponse xmlns="http://ec2.amazonaws.com/doc/2016-11-15/">
                <requestId>fc45294c-006b-457b-bab9-012f5b3b0e40</requestId>
                <networkInterfaceSet>
                   <item>
                     <networkInterfaceId>eni-0f62d866</networkInterfaceId>
                     <subnetId>subnet-c53c87ac</subnetId>
                     <vpcId>vpc-cc3c87a5</vpcId>
                     <availabilityZone>api-southeast-1b</availabilityZone>
                     <description/>
                     <ownerId>053230519467</ownerId>
                     <requesterManaged>false</requesterManaged>
                     <status>in-use</status>
                     <macAddress>02:81:60:cb:27:37</macAddress>
                     <privateIpAddress>10.0.0.146</privateIpAddress>
                     <sourceDestCheck>true</sourceDestCheck>
                     <groupSet>
                       <item>
                         <groupId>sg-3f4b5653</groupId>
                         <groupName>default</groupName>
                       </item>
                     </groupSet>
                     <attachment>
                       <attachmentId>eni-attach-6537fc0c</attachmentId>
                       <instanceId>i-1234567890abcdef0</instanceId>
                       <instanceOwnerId>053230519467</instanceOwnerId>
                       <deviceIndex>0</deviceIndex>
                       <status>attached</status>
                       <attachTime>2012-07-01T21:45:27.000Z</attachTime>
                       <deleteOnTermination>true</deleteOnTermination>
                     </attachment>
                     <tagSet/>
                     <privateIpAddressesSet>
                       <item>
                         <privateIpAddress>10.0.0.146</privateIpAddress>
                         <primary>true</primary>
                       </item>
                       <item>
                         <privateIpAddress>10.0.0.148</privateIpAddress>
                         <primary>false</primary>
                       </item>
                       <item>
                         <privateIpAddress>10.0.0.150</privateIpAddress>
                         <primary>false</primary>
                       </item>
                     </privateIpAddressesSet>
                     <ipv6AddressesSet/>
                   </item>
                   <item>
                     <networkInterfaceId>eni-a66ed5cf</networkInterfaceId>
                     <subnetId>subnet-cd8a35a4</subnetId>
                     <vpcId>vpc-f28a359b</vpcId>
                     <availabilityZone>ap-southeast-1b</availabilityZone>
                     <description>Primary network interface</description>
                     <ownerId>053230519467</ownerId>
                     <requesterManaged>false</requesterManaged>
                     <status>in-use</status>
                     <macAddress>02:78:d7:00:8a:1e</macAddress>
                     <privateIpAddress>10.0.1.233</privateIpAddress>
                     <sourceDestCheck>true</sourceDestCheck>
                     <groupSet>
                       <item>
                         <groupId>sg-a2a0b2ce</groupId>
                         <groupName>quick-start-1</groupName>
                       </item>
                     </groupSet>
                     <attachment>
                       <attachmentId>eni-attach-a99c57c0</attachmentId>
                       <instanceId>i-0598c7d356eba48d7</instanceId>
                       <instanceOwnerId>053230519467</instanceOwnerId>
                       <deviceIndex>0</deviceIndex>
                       <status>attached</status>
                       <attachTime>2012-06-27T20:08:44.000Z</attachTime>
                       <deleteOnTermination>true</deleteOnTermination>
                     </attachment>
                     <tagSet/>
                     <privateIpAddressesSet>
                       <item>
                         <privateIpAddress>10.0.1.233</privateIpAddress>
                         <primary>true</primary>
                       </item>
                       <item>
                         <privateIpAddress>10.0.1.20</privateIpAddress>
                         <primary>false</primary>
                       </item>
                     </privateIpAddressesSet>
                     <ipv6AddressesSet>
                      <item>
                        <ipv6Address>2001:db8:1234:1a00::123</ipv6Address>
                      </item>
                      <item>
                        <ipv6Address>2001:db8:1234:1a00::456</ipv6Address>
                      </item>
                    </ipv6AddressesSet>
                   </item>
                 </networkInterfaceSet>
            </DescribeNetworkInterfacesResponse>
            """);

        Assert.Equal(2, response.NetworkInterfaces.Length);

        var ni = response.NetworkInterfaces[0];

        Assert.Equal("eni-0f62d866", ni.NetworkInterfaceId);
        Assert.Equal("vpc-cc3c87a5", ni.VpcId);
        Assert.Equal("02:81:60:cb:27:37", ni.MacAddress);

        Assert.Equal("eni-attach-6537fc0c", ni.Attachment.AttachmentId);
        Assert.Equal("i-1234567890abcdef0", ni.Attachment.InstanceId);
        Assert.Equal("attached", ni.Attachment.Status);
        Assert.Equal(0, ni.Attachment.DeviceIndex);
    }
}