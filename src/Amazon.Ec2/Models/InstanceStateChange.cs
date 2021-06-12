#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public sealed class InstanceStateChange
    {
        [XmlElement("instanceId")]
        public string InstanceId { get; init; }

        [XmlElement("currentState")]
        public InstanceState CurrentState { get; init; }

        [XmlElement("previousState")]
        public InstanceState PreviousState { get; init; }
    }
}