using System.Net;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

using Amazon.Ec2.Exceptions;
using Amazon.Ec2.Models;
using Amazon.Ec2.Tests;

namespace Amazon.Ec2.Responses.Tests;

public class ResponseParsingTests
{
    private static T Parse<T>(string innerXml)
    {
        return Ec2Serializer<T>.Deserialize(Encoding.UTF8.GetBytes(FakeEc2.Response(typeof(T).Name, innerXml)));
    }

    [Fact]
    public void DescribeInstancesFlattensReservations()
    {
        var response = DescribeInstancesResponse.Deserialize(FakeEc2.Response("DescribeInstancesResponse",
            """
            <reservationSet>
              <item><instancesSet><item><instanceId>i-1</instanceId></item><item><instanceId>i-2</instanceId></item></instancesSet></item>
              <item><instancesSet><item><instanceId>i-3</instanceId></item></instancesSet></item>
            </reservationSet>
            """));

        Assert.Equal(["i-1", "i-2", "i-3"], response.Instances.Select(static i => i.InstanceId));
    }

    [Fact]
    public void DescribeInstancesHandlesNoReservations()
    {
        var response = DescribeInstancesResponse.Deserialize(FakeEc2.Response("DescribeInstancesResponse", "<reservationSet/>"));

        Assert.Empty(response.Instances);
    }

    [Fact]
    public void InstanceReadsArchitecture()
    {
        var response = Parse<RunInstancesResponse>("<instancesSet><item><architecture>x86_64</architecture></item></instancesSet>");

        Assert.Equal("x86_64", response.Instances[0].Architecture);
    }

    [Fact]
    public void ImageReadsOptionalFields()
    {
        var response = Parse<DescribeImagesResponse>(
            """
            <imagesSet>
              <item>
                <imageId>ami-1</imageId>
                <imageType>machine</imageType>
                <imageState>available</imageState>
                <imageLocation>amazon/x</imageLocation>
                <name>x</name>
                <platform>windows</platform>
                <enaSupport>true</enaSupport>
                <creationDate>2019-05-10T13:17:12.000Z</creationDate>
              </item>
            </imagesSet>
            <nextToken>t1</nextToken>
            """);

        Assert.Equal("t1", response.NextToken);

        var image = Assert.Single(response.Images);

        Assert.Equal("machine", image.ImageType);
        Assert.Equal("available", image.ImageState);
        Assert.Equal("amazon/x", image.ImageLocation);
        Assert.Equal("x", image.Name);
        Assert.Equal("windows", image.Platform);
        Assert.True(image.EnaSupport);
        Assert.Equal(new DateTime(2019, 5, 10, 13, 17, 12, DateTimeKind.Utc), image.CreationDate!.Value.ToUniversalTime());
    }

    [Fact]
    public void ImageReadsRamdiskId()
    {
        var response = Parse<DescribeImagesResponse>("<imagesSet><item><ramdiskId>ari-1</ramdiskId></item></imagesSet>");

        Assert.Equal("ari-1", response.Images[0].RamDiskId);
    }

    [Fact]
    public void NetworkInterfaceReadsPrivateDnsName()
    {
        var response = Parse<DescribeNetworkInterfacesResponse>(
            """
            <networkInterfaceSet>
              <item>
                <privateIpAddress>10.0.0.5</privateIpAddress>
                <privateDnsName>ip-10-0-0-5.ec2.internal</privateDnsName>
              </item>
            </networkInterfaceSet>
            """);

        Assert.Equal("10.0.0.5", response.NetworkInterfaces[0].PrivateIpAddress);
        Assert.Equal("ip-10-0-0-5.ec2.internal", response.NetworkInterfaces[0].PrivateDnsName);
    }

    [Fact]
    public void NetworkInterfaceReadsAttachmentAndGroups()
    {
        var response = Parse<DescribeNetworkInterfacesResponse>(
            """
            <networkInterfaceSet>
              <item>
                <networkInterfaceId>eni-1</networkInterfaceId>
                <attachment>
                  <attachmentId>eni-attach-1</attachmentId>
                  <instanceId>i-1</instanceId>
                  <instanceOwnerId>111122223333</instanceOwnerId>
                  <deviceIndex>2</deviceIndex>
                  <status>attached</status>
                  <attachTime>2024-01-02T03:04:05.000Z</attachTime>
                  <deleteOnTermination>true</deleteOnTermination>
                </attachment>
                <groupSet>
                  <item><groupId>sg-1</groupId><groupName>one</groupName></item>
                </groupSet>
              </item>
            </networkInterfaceSet>
            """);

        var networkInterface = Assert.Single(response.NetworkInterfaces);

        Assert.Equal("i-1", networkInterface.Attachment.InstanceId);
        Assert.Equal("111122223333", networkInterface.Attachment.InstanceOwnerId);
        Assert.Equal(2, networkInterface.Attachment.DeviceIndex);
        Assert.True(networkInterface.Attachment.DeleteOnTermination);
        Assert.Equal("one", Assert.Single(networkInterface.Groups).GroupName);
    }

    [Fact]
    public void SecurityGroupReadsReferencedGroups()
    {
        var response = Parse<DescribeSecurityGroupsResponse>(
            """
            <securityGroupInfo>
              <item>
                <ownerId>111122223333</ownerId>
                <groupId>sg-1</groupId>
                <vpcId>vpc-1</vpcId>
                <ipPermissions>
                  <item>
                    <ipProtocol>-1</ipProtocol>
                    <groups>
                      <item>
                        <userId>444455556666</userId>
                        <groupId>sg-2</groupId>
                        <vpcId>vpc-2</vpcId>
                        <vpcPeeringConnectionId>pcx-1</vpcPeeringConnectionId>
                        <peeringStatus>active</peeringStatus>
                      </item>
                    </groups>
                    <ipRanges><item><cidrIp>10.0.0.0/8</cidrIp></item></ipRanges>
                    <ipv6Ranges><item><cidrIpv6>::/0</cidrIpv6></item></ipv6Ranges>
                  </item>
                </ipPermissions>
              </item>
            </securityGroupInfo>
            """);

        var group = Assert.Single(response.SecurityGroups);

        Assert.Equal("vpc-1", group.VpcId);

        var permission = Assert.Single(group.IpPermissions);

        // All-traffic rules have no ports
        Assert.Equal(0, permission.FromPort);
        Assert.Null(permission.ToPort);

        Assert.Equal("10.0.0.0/8", Assert.Single(permission.IpRanges).CidrIp);
        Assert.Equal("::/0", Assert.Single(permission.Ipv6Ranges).CidrIpv6);

        var referenced = Assert.Single(permission.Groups);

        Assert.Equal("444455556666", referenced.UserId);
        Assert.Equal("sg-2", referenced.GroupId);
        Assert.Equal("vpc-2", referenced.VpcId);
        Assert.Equal("pcx-1", referenced.VpcPeeringConnectionId);
        Assert.Equal("active", referenced.PeeringStatus);
    }

    [Fact]
    public void SecurityGroupKeepsLeadingZerosInOwnerId()
    {
        var response = Parse<DescribeSecurityGroupsResponse>("<securityGroupInfo><item><ownerId>012345678901</ownerId></item></securityGroupInfo>");

        Assert.Equal("012345678901", response.SecurityGroups[0].OwnerId);
    }

    [Fact]
    public void DescribeResponsesExposeNextToken()
    {
        const string token = "<nextToken>t1</nextToken>";

        Assert.Equal("t1", DescribeInstancesResponse.Deserialize(FakeEc2.Response("DescribeInstancesResponse", "<reservationSet/>" + token)).NextToken);
        Assert.Equal("t1", Parse<DescribeNetworkInterfacesResponse>("<networkInterfaceSet/>" + token).NextToken);
        Assert.Equal("t1", Parse<DescribeSubnetsResponse>("<subnetSet/>" + token).NextToken);
        Assert.Equal("t1", Parse<DescribeVolumesResponse>("<volumeSet/>" + token).NextToken);
        Assert.Equal("t1", Parse<DescribeVpcsResponse>("<vpcSet/>" + token).NextToken);
    }

    [Fact]
    public void NextTokenIsNullOnTheLastPage()
    {
        Assert.Null(DescribeInstancesResponse.Deserialize(FakeEc2.Response("DescribeInstancesResponse", "<reservationSet/>")).NextToken);
        Assert.Null(Parse<DescribeVolumesResponse>("<volumeSet/>").NextToken);
    }

    [Fact]
    public void AddressCanBeDeserialized()
    {
        var serializer = new XmlSerializer(typeof(Address), new XmlRootAttribute("item"));

        using var reader = new StringReader("<item><allocationId>eipalloc-1</allocationId><associationId>eipassoc-1</associationId></item>");

        var address = (Address)serializer.Deserialize(reader)!;

        Assert.Equal("eipalloc-1", address.AllocationId);
        Assert.Equal("eipassoc-1", address.AssociationId);
    }

    [Fact]
    public void InstanceTypeInfoCanBeReadFromAnElement()
    {
        XNamespace ns = Ec2Client.Namespace;

        var element = new XElement(ns + "item",
            new XElement(ns + "instanceType", "t2.micro"),
            new XElement(ns + "hypervisor", "xen"),
            new XElement(ns + "freeTierEligible", "true"));

        var info = InstanceTypeInfo.Deserialize(element);

        Assert.Equal("t2.micro", info.InstanceType);
        Assert.Equal("xen", info.Hypervisor);
        Assert.True(info.FreeTierEligible);
    }

    [Fact]
    public void ErrorResponseThrowsOnNonXml()
    {
        Assert.ThrowsAny<InvalidOperationException>(() => ErrorResponse.Deserialize("not xml"));
    }
}

public class Ec2ExceptionTests
{
    [Fact]
    public void MessageConstructorHasNoErrors()
    {
        var ex = new Ec2Exception("failed", HttpStatusCode.BadGateway);

        Assert.Equal("failed", ex.Message);
        Assert.Equal(HttpStatusCode.BadGateway, ex.HttpStatusCode);
        Assert.Null(ex.Errors);
    }

    [Fact]
    public void MultipleErrorsUseASummaryMessage()
    {
        Error[] errors = [
            new Error { Code = "A", Message = "a" },
            new Error { Code = "B", Message = "b" }
        ];

        var ex = new Ec2Exception(errors, HttpStatusCode.BadRequest);

        Assert.Equal("Multiple errors occurred processing the request. See inner Errors[] for details.", ex.Message);
        Assert.Same(errors, ex.Errors);
    }
}
