#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public class CreateListenerResponse : IElbResponse
    {
        [XmlElement]
        public CreateListenerResult CreateListenerResult { get; set; }
    }

    public class CreateListenerResult
    {
        [XmlArray]
        [XmlArrayItem("member")]
        public Listener[] Listeners { get; set; }
    }
}
