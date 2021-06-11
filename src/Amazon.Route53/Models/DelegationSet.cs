#nullable disable

using System.Xml.Serialization;

namespace Amazon.Route53
{
    public sealed class DelegationSet
    {
        public string CallerReference { get; init; }

        public string Id { get; init; }

        [XmlArray]
        [XmlArrayItem("NameServer")]
        public string[] NameServers { get; init; }
    }
}
