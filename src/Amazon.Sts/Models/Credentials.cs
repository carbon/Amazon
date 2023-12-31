using System.Xml.Serialization;

namespace Amazon.Sts;

public sealed class Credentials
{
    [XmlElement]
    public required string AccessKeyId { get; init; }

    [XmlElement]
    public required string SecretAccessKey { get; init; }

    [XmlElement]
    public required DateTime Expiration { get; init; }

    [XmlElement]
    public required string SessionToken { get; init; }

    public TimeSpan ExpiresIn => Expiration - DateTime.UtcNow;
}