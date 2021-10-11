#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sts;

public class Credentials
{
    [XmlElement]
    public string SessionToken { get; set; }

    [XmlElement]
    public string SecretAccessKey { get; set; }

    [XmlElement]
    public string Expiration { get; set; }

    [XmlElement]
    public string AccessKeyId { get; set; }
}