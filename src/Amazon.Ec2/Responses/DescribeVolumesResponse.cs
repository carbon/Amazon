using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Amazon.Ec2
{
    using System.IO;

    [XmlRoot("DescribeVolumesResponse", Namespace = "http://ec2.amazonaws.com/doc/2016-09-15/")]
    public class DescribeVolumesResponse
    {
        [XmlArray("volumeSet")]
        [XmlArrayItem("item")]
        public List<Volume> Volumes { get; } = new List<Volume>();

        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(DescribeVolumesResponse));

        public static DescribeVolumesResponse Parse(string text)
        {
            using (var reader = new StringReader(text))
            {
                return (DescribeVolumesResponse)serializer.Deserialize(reader);
            }          
        }
    }
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