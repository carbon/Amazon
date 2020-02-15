#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public class Cipher
    {
        [XmlElement]
        public string Name { get; set; }

        [XmlElement]
        public int Priority { get; set; }
    }
}
