#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sts
{
    public class DecodeAuthorizationMessageResponse : IStsResponse
    {
        [XmlElement]
        public string DecodedMessage { get; set; }
    }
}