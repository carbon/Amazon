#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public sealed class InstanceStateChange
    {
        [XmlElement("instanceId")]
        public string InstanceId { get; set; }

        [XmlElement("currentState")]
        public InstanceState CurrentState { get; set; }

        [XmlElement("previousState")]
        public InstanceState PreviousState { get; set; }
    }
}