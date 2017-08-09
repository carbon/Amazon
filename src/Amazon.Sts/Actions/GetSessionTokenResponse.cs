using System.Xml.Serialization;

namespace Amazon.Sts
{
    public class GetSessionTokenResponse : IStsResponse
    {
        [XmlElement]
        public GetSessionTokenResult GetSessionTokenResult { get; set; }
    }

    public class GetSessionTokenResult
    {
        [XmlElement]
        public Credentials Credentials { get; set; }
    }
}