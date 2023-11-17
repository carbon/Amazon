using System.Globalization;

namespace Amazon.Ec2.Responses.Tests;

public class DescribeVolumesResponseTests
{
    [Fact]
    public void CanDeserialize()
    {
        var response = Ec2Serializer<DescribeVolumesResponse>.Deserialize(
            """
            <DescribeVolumesResponse xmlns="http://ec2.amazonaws.com/doc/2016-11-15/">
               <requestId>59dbff89-35bd-4eac-99ed-be587EXAMPLE</requestId> 
               <volumeSet>
                  <item>
                     <volumeId>vol-1234567890abcdef0</volumeId>
                     <size>80</size>
                     <snapshotId/>
                     <availabilityZone>us-east-1a</availabilityZone>
                     <status>in-use</status>
                     <createTime>2016-01-05T03:15:30Z</createTime>
                     <attachmentSet>
                        <item>
                           <volumeId>vol-1234567890abcdef0</volumeId>
                           <instanceId>i-1234567890abcdef0</instanceId>
                           <device>/dev/sdh</device>
                           <status>attached</status>
                           <attachTime>2016-01-05T03:15:30Z</attachTime>
                           <deleteOnTermination>false</deleteOnTermination>
                        </item>
                     </attachmentSet>
                     <volumeType>standard</volumeType>
                     <encrypted>true</encrypted>
                  </item>
               </volumeSet>
            </DescribeVolumesResponse>
            """);

        Assert.Single(response.Volumes);

        var volume = response.Volumes[0];
        var date = DateTime.Parse("2016-01-05T03:15:30Z", null, DateTimeStyles.AdjustToUniversal);

        Assert.Equal(date, volume.CreateTime);
        Assert.Equal("vol-1234567890abcdef0", volume.VolumeId);
        Assert.Equal("us-east-1a", volume.AvailabilityZone);
        Assert.Equal("in-use", volume.Status);
        Assert.Equal(80, volume.Size);
        Assert.Equal("standard", volume.VolumeType);

        var attachment_0 = volume.Attachments[0];

        Assert.Equal("vol-1234567890abcdef0", attachment_0.VolumeId);
        Assert.Equal("i-1234567890abcdef0", attachment_0.InstanceId);
        Assert.Equal("/dev/sdh", attachment_0.Device);

        Assert.Equal(date, attachment_0.AttachTime);

        Assert.Single(volume.Attachments);
    }
}