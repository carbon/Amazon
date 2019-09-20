#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ses
{
    public class SesError
    {
        [XmlElement]
        public string Type { get; set; }

        [XmlElement]
        public string Code { get; set; }

        [XmlElement]
        public string Message { get; set; }
    }
}