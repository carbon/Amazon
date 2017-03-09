using System;
using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public class NetworkInterfaceAttachment
    {
        [XmlElement("attachmentId")]
        public string AttachmentId  { get; set; }

        [XmlElement("instanceId")]
        public string InstanceId { get; set; }
      
        [XmlElement("instanceOwnerId")]
        public string InstanceOwnerId  { get; set; }

        [XmlElement("deviceIndex")]
        public int DeviceIndex { get; set; }

        [XmlElement("status")]
        public string Status  { get; set; }

        [XmlElement("attachTime")]
        public DateTime AttachTime { get; set; }

        [XmlElement("deleteOnTermination")]
        public bool DeleteOnTermination { get; set; }
    }
}

/*
<attachment>
    <attachmentId>eni-attach-6537fc0c</attachmentId>
    <instanceId>i-1234567890abcdef0</instanceId>
    <instanceOwnerId>053230519467</instanceOwnerId>
    <deviceIndex>0</deviceIndex>
    <status>attached</status>
    <attachTime>2012-07-01T21:45:27.000Z</attachTime>
    <deleteOnTermination>true</deleteOnTermination>
</attachment>
*/