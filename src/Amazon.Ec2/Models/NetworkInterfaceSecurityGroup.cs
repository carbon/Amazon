using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public class NetworkInterfaceSecurityGroup
    {
#nullable disable
        public NetworkInterfaceSecurityGroup() { }
#nullable enable

        public NetworkInterfaceSecurityGroup(string groupId, string groupName)
        {
            GroupId = groupId;
            GroupName = groupName;
        }

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