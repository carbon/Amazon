#nullable disable

using System.Xml.Serialization;

namespace Amazon.S3
{
    public sealed class Owner
    {
        [XmlElement("ID")]
        public string ID { get; set; }

        [XmlElement("DisplayName")]
        public string DisplayName { get; set; }
    }
}