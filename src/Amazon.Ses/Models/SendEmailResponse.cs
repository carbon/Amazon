#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ses;

[XmlRoot(Namespace = SesClient.Namespace)]
public sealed class SendEmailResponse
{
    [XmlElement]
    public SendEmailResult SendEmailResult { get; init; }

    public static SendEmailResponse Parse(string text)
    {
        return XmlHelper<SendEmailResponse>.Deserialize(text);
    }
}

public sealed class SendEmailResult
{
    [XmlElement]
    public string MessageId { get; init; }
}
