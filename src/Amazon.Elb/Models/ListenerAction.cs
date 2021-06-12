#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public sealed class ListenerAction
    {
        [XmlElement]
        public string Type { get; init; }

        [XmlElement]
        public string TargetGroupArn { get; init; }
    }
}