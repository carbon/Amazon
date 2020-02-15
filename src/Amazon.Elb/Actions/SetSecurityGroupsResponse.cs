#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public class SetSecurityGroupsResponse : IElbResponse
    {
        [XmlElement]
        public SetSecurityGroupsResult SetSecurityGroupsResult { get; set; }
    }

    public class SetSecurityGroupsResult
    {
        [XmlArray]
        [XmlArrayItem("member")]
        public string[] SecurityGroupIds { get; set; }
    }
}