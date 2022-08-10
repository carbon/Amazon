#nullable disable

using System.Xml.Serialization;

namespace Amazon.S3;

[XmlRoot("Error")]
public sealed class S3Error
{
    [XmlElement]
    public string Code { get; init; }

    [XmlElement]
    public string Message { get; init; }

    [XmlElement]
    public string Resource { get; init; }

    [XmlElement]
    public string RequestId { get; init; }

    [XmlElement]
    public string HostId { get; init; }

    // RangeRequested
    // ActualObjectSize

    public static S3Error Deserialize(string xmlText)
    {
        return S3Serializer<S3Error>.Deserialize(xmlText);
    }

    internal static bool TryDeserialize(string xmlText, out S3Error error)
    {
        return S3Serializer<S3Error>.TryDeserialize(xmlText, out error);
    }
}

// https://docs.aws.amazon.com/AmazonS3/latest/API/ErrorResponses.html

/*
<?xml version="1.0" encoding="UTF-8"?>
<Error>
	<Code>NoSuchKey</Code>
	<Message>The resource you requested does not exist</Message>
	<Resource>/mybucket/myfoto.jpg</Resource> 
	<RequestId>4442587FB7D0A2F9</RequestId>
 	<HostId>4PsK3Ki9G28+pJeh0c3jo3V2sqnftQ5DROhs+U9p4SaJk4BHmjvB2xZfDUgIuENf</HostId>
</Error>
*/