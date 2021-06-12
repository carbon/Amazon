#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public sealed class LoadBalancerAttribute
    {
        public LoadBalancerAttribute() { }

        public LoadBalancerAttribute(string key, string value)
        {
            Key = key;
            Value = value;
        }

        [XmlElement]
        public string Key { get; init; }

        [XmlElement]
        public string Value { get; init; }
    }
}