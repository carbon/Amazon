#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public sealed class Cipher
    {
        [XmlElement]
        public string Name { get; init; }

        [XmlElement]
        public int Priority { get; init; }
    }
}
