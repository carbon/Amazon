namespace Amazon.Ec2.Responses.Tests;

public class DescribeImagesResponseTests
{
    [Fact]
    public void CanDeserialize()
    {
        var response = Ec2Serializer<DescribeImagesResponse>.Deserialize(
            """
            <DescribeImagesResponse xmlns="http://ec2.amazonaws.com/doc/2016-11-15/">
              <requestId>59dbff89-35bd-4eac-99ed-be587EXAMPLE</requestId> 
              <imagesSet>
                <item>
                  <imageId>ami-1a2b3c4d</imageId>
                  <imageLocation>amazon/getting-started</imageLocation>
                  <imageState>available</imageState>
                  <imageOwnerId>123456789012</imageOwnerId>
                  <isPublic>true</isPublic>
                  <architecture>i386</architecture>
                  <imageType>machine</imageType>
                  <kernelId>aki-1a2b3c4d</kernelId>
                  <ramdiskId>ari-1a2b3c4d</ramdiskId>
                  <imageOwnerAlias>amazon</imageOwnerAlias>
                  <imageOwnerId>123456789012</imageOwnerId>
                  <name>getting-started</name>
                  <description>Image Description</description>
                  <rootDeviceType>ebs</rootDeviceType>
                  <rootDeviceName>/dev/sda</rootDeviceName>
                  <blockDeviceMapping>
                    <item>
                      <deviceName>/dev/sda1</deviceName>
                      <ebs>
                        <snapshotId>snap-1234567890abcdef0</snapshotId>
                        <volumeSize>15</volumeSize>
                        <deleteOnTermination>false</deleteOnTermination>
                        <volumeType>standard</volumeType>
                      </ebs>
                    </item>
                  </blockDeviceMapping>
                  <virtualizationType>paravirtual</virtualizationType>
                  <tagSet/>
                  <hypervisor>xen</hypervisor>
                </item>
              </imagesSet>
            </DescribeImagesResponse>
            """);

        Assert.Single(response.Images);

        var image = response.Images[0];

        Assert.Equal(123456789012, image.ImageOwnerId);
        Assert.Equal("amazon", image.ImageOwnerAlias);
        Assert.Equal("ami-1a2b3c4d", image.ImageId);
        Assert.Equal("i386", image.Architecture);
        Assert.Equal("aki-1a2b3c4d", image.KernelId);
        Assert.Equal("Image Description", image.Description);
        Assert.Equal("paravirtual", image.VirtualizationType);
        Assert.Equal("ebs", image.RootDeviceType);
        Assert.Equal("/dev/sda", image.RootDeviceName);

        Assert.Equal("xen", image.Hypervisor);

        Assert.True(image.IsPublic);

        // Assert.True("/dev/sda1", image.blockDeviceMappings[0]);
    }
}