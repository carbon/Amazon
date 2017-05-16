using System;
using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public class BlockDeviceMapping
    {
        public BlockDeviceMapping() { }

        public BlockDeviceMapping(string deviceName, EbsBlockDevice ebs)
        {
            DeviceName = deviceName ?? throw new ArgumentNullException(nameof(deviceName));
            Ebs        = ebs ?? throw new ArgumentNullException(nameof(ebs));
        }

        [XmlElement("deviceName")]
        public string DeviceName { get; set; }

        [XmlElement("ebs")]
        public EbsBlockDevice Ebs { get; set; }

        [XmlElement("virtualName")]
        public string VirtualName { get; set; }
    }

    public static class BlockDeviceNames
    {
        public const string Root = "/dev/sda1";

        public const string sda1 = "/dev/sda1";
        public const string xvda = "/dev/xvda";
    }

}


/*
<deviceName>/dev/xvda</deviceName>
<ebs>
    <volumeId>vol-1234567890abcdef0</volumeId>
    <status>attached</status>
    <attachTime>2015-12-22T10:44:09.000Z</attachTime>
    <deleteOnTermination>true</deleteOnTermination>
</ebs>
*/
