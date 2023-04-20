#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ses;

[XmlRoot(Namespace = SesClient.Namespace)]
public sealed class ErrorResponse
{
    [XmlElement]
    public SesError Error { get; init; }

    public static ErrorResponse Deserialize(string text)
    {
        return SesSerializer<ErrorResponse>.Deserialize(text);
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