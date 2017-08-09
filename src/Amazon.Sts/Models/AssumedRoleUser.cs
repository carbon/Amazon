using System.Xml.Serialization;

namespace Amazon.Sts
{
    public class AssumedRoleUser
    {
        [XmlElement]
        public string Arn { get; set; }

        [XmlElement]
        public string AssumedRoleId { get; set; }
    }
}