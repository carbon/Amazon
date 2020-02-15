#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public class ListenerAction
    {
        [XmlElement]
        public string Type { get; set; }

        [XmlElement]
        public string TargetGroupArn { get; set; }
    }
}