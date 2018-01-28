using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public class ProductCode
    {
        [XmlElement("productCode")]
        public string Value { get; set; }

        [XmlElement("type")]
        public string Type { get; set; }
    }
}
