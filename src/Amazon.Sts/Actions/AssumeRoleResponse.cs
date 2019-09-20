#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sts
{
    public class AssumeRoleResponse : IStsResponse
    {
        [XmlElement]
        public AssumeRoleResult AssumeRoleResult { get; set; }
    }

    public class AssumeRoleResult
    {
        [XmlElement]
        public Credentials Credentials { get; set; }

        [XmlElement]
        public AssumedRoleUser AssumedRoleUser { get; set; }
    }
}