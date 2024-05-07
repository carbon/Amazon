using System.Xml.Serialization;

namespace Amazon.S3;

[XmlRoot("DeleteResult", Namespace = S3Client.Namespace)]
public sealed class DeleteResult
{
    [XmlElement("Deleted")]
    public BatchItem[]? Deleted { get; init; }

    [XmlElement("Error")]
    public DeleteResultError[]? Errors { get; init; }

    [XmlIgnore]
    public bool HasErrors => Errors is { Length: > 0 };
}

#nullable disable

public readonly struct BatchItem
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