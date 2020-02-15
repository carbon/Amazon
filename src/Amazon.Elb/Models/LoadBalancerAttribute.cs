#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public class LoadBalancerAttribute
    {
        [XmlElement]
        public string Key { get; set; }

        [XmlElement]
        public string Value { get; set; }
    }
}