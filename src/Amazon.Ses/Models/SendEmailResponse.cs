#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ses
{
    [XmlRoot(Namespace = SesClient.Namespace)]
    public sealed class SendEmailResponse
    {
        [XmlElement]
        public SendEmailResult SendEmailResult { get; set; }

        public static SendEmailResponse Parse(string text)
        {
            return XmlHelper<SendEmailResponse>.Deserialize(text);
        }
    }

    public class SendEmailResult
    {
        [XmlElement]
        public string MessageId { get; set; }
    }
}

/*
<SendEmailResponse xmlns="http://ses.amazonaws.com/doc/2010-12-01/">
  <SendEmailResult>
    <MessageId>00000131d51d2292-159ad6eb-077c-46e6-ad09-ae7c05925ed4-000000</MessageId>
  </SendEmailResult>
  <ResponseMetadata>
    <RequestId>d5964849-c866-11e0-9beb-01a62d68c57f</RequestId>
  </ResponseMetadata>
</SendEmailResponse>
*/
