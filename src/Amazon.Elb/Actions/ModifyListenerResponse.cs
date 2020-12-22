﻿#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public sealed class ModifyListenerResponse : IElbResponse
    {
        [XmlElement]
        public ModifyListenerResult ModifyListenerResult { get; set; }
    }

    public sealed class ModifyListenerResult
    {
        [XmlArray]
        [XmlArrayItem("member")]
        public Listener[] Listeners { get; set; }
    }
}