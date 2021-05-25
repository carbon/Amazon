#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sqs.Models
{
    public sealed class ErrorResponse
    {
        [XmlElement("Error")]
        public SqsError Error { get; set; }

        [XmlElement("RequestId")]
        public string RequestId { get; set; }

        public static ErrorResponse ParseXml(string xmlText)
        {
            return SqsSerializer<ErrorResponse>.Deserialize(xmlText);
        }
    }
}

/*
<ErrorResponse>
   <Error>
      <Type>Sender</Type>
      <Code>InvalidParameterValue</Code>
      <Message>
         Value (quename_nonalpha) for parameter QueueName is invalid.
         Must be an alphanumeric String of 1 to 80 in length
      </Message>
   </Error>
   <RequestId>
      42d59b56-7407-4c4a-be0f-4c88daeea257
   </RequestId>
</ErrorResponse>
*/
