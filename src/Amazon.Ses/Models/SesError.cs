using System.Xml.Serialization;

namespace Amazon.Ses
{
    using Helpers;

    [XmlRoot(Namespace = "http://ses.amazonaws.com/doc/2010-12-01/")]
    public class ErrorResponse
    {
        [XmlElement]
        public SesError Error { get; set; }

        public static ErrorResponse Parse(string text)
        {
            return XmlText.ToObject<ErrorResponse>(text);
        }
    }

    public class SesError
    {
        [XmlElement]
        public string Type { get; set; }

        [XmlElement]
        public string Code { get; set; }

        [XmlElement]
        public string Message { get; set; }
    }
}

/*
<ErrorResponse xmlns="http://ses.amazonaws.com/doc/2010-12-01/">
  <Error>
    <Type>Sender</Type>
    <Code>InvalidParameterValue</Code>
    <Message>Local address contains control or whitespace</Message>
  </Error>
  <RequestId>0de719f7-7cde-11e3-8c9d-5942f9840c3a</RequestId>
</ErrorResponse>
 */
