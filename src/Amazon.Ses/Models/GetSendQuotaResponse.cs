#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ses;

[XmlRoot(Namespace = SesClient.Namespace)]
public sealed class GetSendQuotaResponse
{
    [XmlElement]
    public GetSendQuotaResult GetSendQuotaResult { get; init; }

    public static GetSendQuotaResponse Deserialize(string text)
    {
        return SesSerializer<GetSendQuotaResponse>.Deserialize(text);
    }
}

public sealed class GetSendQuotaResult
{
    [XmlElement]
    public float SentLast24Hours { get; init; }

    [XmlElement]
    public float Max24HourSend { get; init; }

    [XmlElement]
    public float MaxSendRate { get; init; }
}

/*
                
<GetSendQuotaResponse xmlns="http://ses.amazonaws.com/doc/2010-12-01/">
  <GetSendQuotaResult>
    <SentLast24Hours>127.0</SentLast24Hours>
    <Max24HourSend>200.0</Max24HourSend>
    <MaxSendRate>1.0</MaxSendRate>
  </GetSendQuotaResult>
  <ResponseMetadata>
    <RequestId>273021c6-c866-11e0-b926-699e21c3af9e</RequestId>
  </ResponseMetadata>
</GetSendQuotaResponse>

*/
