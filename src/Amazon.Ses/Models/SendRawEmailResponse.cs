#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ses;

[XmlRoot(Namespace = SesClient.Namespace)]
public sealed class SendRawEmailResponse
{
    [XmlElement]
    public SendRawEmailResult SendRawEmailResult { get; init; }

    public static SendRawEmailResponse Parse(string text)
    {
        return XmlHelper<SendRawEmailResponse>.Deserialize(text);
    }
}

public sealed class SendRawEmailResult
{
    [XmlElement]
    public string MessageId { get; init; }
}