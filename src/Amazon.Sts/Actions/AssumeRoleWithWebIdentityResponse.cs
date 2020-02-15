#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sts
{
    public sealed class AssumeRoleWithWebIdentityResponse : IStsResponse
    {
        [XmlElement]
        public AssumeRoleWithWebIdentityResult AssumeRoleWithWebIdentityResult { get; set; }
    }


    public class AssumeRoleWithWebIdentityResult
    {
        [XmlElement]
        public string SubjectFromWebIdentityToken { get; set; }

        [XmlElement]
        public string Audience { get; set; }

        [XmlElement]
        public Credentials Credentials { get; set; }

        [XmlElement]
        public string Provider { get; set; }
    }
}