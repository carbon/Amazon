#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ses;

[XmlRoot(Namespace = SesClient.Namespace)]
public sealed class SendRawEmailResponse
{
    [XmlElement]
    public SendRawEmailResult SendRawEmailResult { get; init; }

    public static SendRawEmailResponse Deserialize(byte[] xmlText)
    {
        return SesSerializer<SendRawEmailResponse>.Deserialize(xmlText);
    }
}

public sealed class SendRawEmailResult
{
    [XmlElement]
    public string MessageId { get; init; }
}