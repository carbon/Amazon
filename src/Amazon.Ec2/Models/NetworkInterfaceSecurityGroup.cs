using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public class NetworkInterfaceSecurityGroup
    {
        [XmlElement("groupId")]
        public string GroupId { get; set; }

        [XmlElement("groupName")]
        public string GroupName { get; set; }
    }
}

/*
<item>
    <groupId>sg-e4076980</groupId>
    <groupName>SecurityGroup1</groupName>
</item>
*/