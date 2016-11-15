using System.Xml.Serialization;

namespace Amazon.Ec2
{
    public class InstanceState
    {
        /*
        0 : pending
        16 : running
        32 : shutting-down
        48 : terminated
        64 : stopping
        80 : stopped
        */
        [XmlElement("code")]
        public int Code { get; set; }

        // Valid Values: pending | running | shutting-down | terminated | stopping | stopped

        [XmlElement("name")]
        public string Name { get; set; }
    }
}