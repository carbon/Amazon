#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public sealed class SslPolicy
    {
        [XmlElement]
        public string Name { get; init; }

        [XmlArray]
        [XmlArrayItem("member")]
        public Cipher[] Ciphers { get; init; }

        [XmlArray]
        [XmlArrayItem("member")]
        public string[] SslProtocols { get; init; }
    }
}
