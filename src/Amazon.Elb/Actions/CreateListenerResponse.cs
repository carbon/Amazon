#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public sealed class CreateListenerResponse : IElbResponse
    {
        [XmlElement]
        public CreateListenerResult CreateListenerResult { get; init; }
    }

    public sealed class CreateListenerResult
    {
        [XmlArray]
        [XmlArrayItem("member")]
        public Listener[] Listeners { get; init; }
    }
}
