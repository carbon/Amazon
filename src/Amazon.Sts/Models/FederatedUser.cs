using System.Xml.Serialization;

namespace Amazon.Sts
{
    public class FederatedUser
    {
        [XmlElement]
        public string Arn { get; set; }

        [XmlElement]
        public string FederatedUserId { get; set; }
    }
}