using System.Xml.Serialization;

namespace Amazon.Ec2;

public sealed class BlockDeviceMapping
{
#nullable disable
    public BlockDeviceMapping() { }
#nullable enable

    public BlockDeviceMapping(string deviceName, EbsBlockDevice ebs)
    {
        DeviceName = deviceName;
        Ebs = ebs;
    }

    [XmlElement("deviceName")]
    public string DeviceName { get; set; }

    [XmlElement("ebs")]
    public EbsBlockDevice Ebs { get; set; }

    [XmlElement("virtualName")]
    public string? VirtualName { get; set; }
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
