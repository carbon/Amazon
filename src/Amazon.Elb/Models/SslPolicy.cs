#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public class SslPolicy
    {
        [XmlElement]
        public string Name { get; set; }

        [XmlArray]
        [XmlArrayItem("member")]
        public Cipher[] Ciphers { get; set; }

        [XmlArray]
        [XmlArrayItem("member")]
        public string[] SslProtocols { get; set; }
    }
}
