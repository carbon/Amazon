using System.Xml.Serialization;


namespace Amazon.Sts
{
    public class GetFederationTokenResponse : IStsResponse
    {
        [XmlElement]
        public GetFederationTokenResult GetFederationTokenResult { get; set; }
    }

    public class GetFederationTokenResult
    {
        [XmlElement]
        public Credentials Credentials { get; set; }

        [XmlElement]
        public FederatedUser FederatedUser { get; set; }

        [XmlElement]
        public int PackedPolicySize { get; set; }
    }
}