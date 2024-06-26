﻿#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ses;

[XmlRoot(Namespace = SesClient.Namespace)]
public sealed class SendEmailResponse
{
    [XmlElement]
    public SendEmailResult SendEmailResult { get; init; }

    public static SendEmailResponse Deserialize(byte[] text)
    {
        return SesSerializer<SendEmailResponse>.Deserialize(text);
    }
}

public sealed class SendEmailResult
{
    [XmlElement]
    public string MessageId { get; init; }
}
