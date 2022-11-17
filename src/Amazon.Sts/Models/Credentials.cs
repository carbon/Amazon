#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sts;

public sealed class Credentials
{
    [XmlElement]
    public string SessionToken { get; init; }

    [XmlElement]
    public string SecretAccessKey { get; init; }

    [XmlElement]
    public DateTime Expiration { get; init; }

    [XmlElement]
    public string AccessKeyId { get; init; }

    public TimeSpan ExpiresIn => Expiration - DateTime.UtcNow;
}