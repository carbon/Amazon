#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public sealed class SetSubnetsResponse : IElbResponse
    {
        [XmlElement]
        public SetSubnetsResult SetSubnetsResult { get; init; }
    }

    public sealed class SetSubnetsResult
    {
        [XmlArray]
        [XmlArrayItem("member")]
        public AvailabilityZone[] AvailabilityZones { get; init; }
    }
}