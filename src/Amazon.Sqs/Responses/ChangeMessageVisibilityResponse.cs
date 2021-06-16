#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sqs.Models
{
    public sealed class ChangeMessageVisibilityResponse
    {
        [XmlElement("ResponseMetadata")]
        public ResponseMetadata ResponseMetadata { get; init; }
    }
}

/*
<ChangeMessageVisibilityResponse>
    <ResponseMetadata>
        <RequestId>6a7a282a-d013-4a59-aba9-335b0fa48bed</RequestId>
    </ResponseMetadata>
</ChangeMessageVisibilityResponse>
*/