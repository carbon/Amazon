#nullable disable

using System;
using System.Xml.Serialization;

namespace Amazon.S3;

public sealed class ObjectVersion
{
    [XmlElement("Key")]
    public string Key { get; init; }

    [XmlElement("ETag")]
    public string ETag { get; init; }

    [XmlElement("IsLatest")]
    public bool IsLatest { get; init; }

    [XmlElement("LastModified", DataType = "dateTime")]
    public DateTime LastModified { get; init; }

    [XmlElement("Size")]
    public long Size { get; init; }

    [XmlElement("StorageClass")]
    public string StorageClass { get; init; }

    [XmlElement("VersionId")]
    public string VersionId { get; init; }

    [XmlElement("Owner")]
    public Owner Owner { get; init; }
}