using System.Xml.Linq;

namespace Amazon.Sqs.Models
{
    public class ErrorResponse
    {
        public SqsError Parse(string xmlText)
        {
            var errorResponseEl = XElement.Parse(xmlText);
            var errorEl = errorResponseEl.Element("Error");

            return new SqsError {
                Type = errorEl.Element("Type").Value,
                Code = errorEl.Element("Code").Value,
                Message = errorEl.Element("Message").Value
            };
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
