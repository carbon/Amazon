#nullable disable

using System.Xml.Serialization;

namespace Amazon.S3;

public sealed class DeleteMarkerEntry
{
    [XmlElement("Key")]
    public string Key { get; init; }

    [XmlElement("IsLatest")]
    public bool IsLatest { get; init; }

    [XmlElement("LastModified", DataType = "dateTime")]
    public DateTime LastModified { get; init; }

    [XmlElement("VersionId")]
    public string VersionId { get; init; }
}