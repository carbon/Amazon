using System.Xml.Serialization;

namespace Amazon.Elb
{
    public class ModifyListenerResponse : IElbResponse
    {
        [XmlElement]
        public ModifyListenerResult ModifyListenerResult { get; set; }
    }

    public class ModifyListenerResult
    {
        [XmlArray]
        [XmlArrayItem("member")]
        public Listener[] Listeners { get; set; }
    }
}