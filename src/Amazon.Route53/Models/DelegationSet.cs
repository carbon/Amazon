#nullable disable

using System.Xml.Serialization;

namespace Amazon.Route53
{
    public class DelegationSet
    {
        public string CallerReference { get; set; }

        public string Id { get; set; }

        [XmlArray]
        [XmlArrayItem("NameServer")]
        public string[] NameServers { get; set; }


    }
}
