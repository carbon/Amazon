using System.Xml.Serialization;

namespace Amazon.S3;

[XmlRoot("DeleteResult", Namespace = S3Client.Namespace)]
public sealed class DeleteResult
{
    [XmlElement("Deleted")]
    public BatchItem[]? Deleted { get; init; }

    [XmlElement("Error")]
    public DeleteResultError[]? Errors { get; init; }

    public bool HasErrors => Errors is { Length: > 0 };

    public static DeleteResult ParseXml(string xmlText)
    {
        return ResponseHelper<DeleteResult>.ParseXml(xmlText);
    }
}

#nullable disable

public sealed class BatchItem
{
    public BatchItem() { }

    public BatchItem(string key)
    {
        Key = key;
    }

    [XmlElement]
    public string Key { get; init; }
}

public sealed class DeleteResultError
{
    [XmlElement]
    public string Key { get; init; }

    [XmlElement]
    public string Code { get; init; }

    [XmlElement]
    public string Message { get; init; }
}

/*
<?xml version="1.0" encoding="UTF-8"?>
<DeleteResult xmlns="http://s3.amazonaws.com/doc/2006-03-01/">
	<Deleted>
		<Key>sample1.txt</Key>
	</Deleted>
	<Error>
		<Key>sample2.txt</Key>
		<Code>AccessDenied</Code>
		<Message>Access Denied</Message>
	</Error>
</DeleteResult>
*/
