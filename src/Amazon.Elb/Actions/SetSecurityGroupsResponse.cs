#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public sealed class SetSecurityGroupsResponse : IElbResponse
    {
        [XmlElement]
        public SetSecurityGroupsResult SetSecurityGroupsResult { get; set; }
    }

    public sealed class SetSecurityGroupsResult
    {
        [XmlArray]
        [XmlArrayItem("member")]
        public string[] SecurityGroupIds { get; set; }
    }
}