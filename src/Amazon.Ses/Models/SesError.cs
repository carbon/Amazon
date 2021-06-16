#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ses
{
    public sealed class SesError
    {
        [XmlElement]
        public string Type { get; init; }

        [XmlElement]
        public string Code { get; init; }

        [XmlElement]
        public string Message { get; init; }
    }
}