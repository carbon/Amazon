#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2;

public sealed class DescribeVolumesResponse : IEc2Response
{
    [XmlArray("volumeSet")]
    [XmlArrayItem("item")]
    public Volume[] Volumes { get; init; }
}

/*
<DescribeVolumesResponse xmlns="http://ec2.amazonaws.com/doc/2016-11-15/">
   <requestId>59dbff89-35bd-4eac-99ed-be587EXAMPLE</requestId> 
   <volumeSet>
      <item>
         <volumeId>vol-1234567890abcdef0</volumeId>
         <size>80</size>
         <snapshotId/>
         <availabilityZone>us-east-1a</availabilityZone>
         <status>in-use</status>
         <createTime>YYYY-MM-DDTHH:MM:SS.SSSZ</createTime>
         <attachmentSet>
            <item>
               <volumeId>vol-1234567890abcdef0</volumeId>
               <instanceId>i-1234567890abcdef0</instanceId>
               <device>/dev/sdh</device>
               <status>attached</status>
               <attachTime>YYYY-MM-DDTHH:MM:SS.SSSZ</attachTime>
               <deleteOnTermination>false</deleteOnTermination>
            </item>
         </attachmentSet>
         <volumeType>standard</volumeType>
         <encrypted>true</encrypted>
      </item>
   </volumeSet>
</DescribeVolumesResponse>
*/