#nullable disable

using System.Xml.Serialization;

namespace Amazon.S3;

[XmlRoot("ListVersionsResult", Namespace = S3Client.Namespace)]
public sealed class ListVersionsResult
{
    [XmlElement("Name")]
    public string Name { get; init; }

    [XmlElement("KeyMarker")]
    public string KeyMarker { get; init; }

    [XmlElement("MaxKeys")]
    public int MaxKeys { get; init; }

    [XmlElement("Prefix")]
    public string Prefix { get; init; }

    [XmlElement("VersionIdMarker")]
    public string VersionIdMarker { get; init; }

    [XmlElement("IsTruncated")]
    public bool IsTruncated { get; init; }

#nullable enable
    [XmlElement("DeleteMarker")]
    public DeleteMarkerEntry[]? DeleteMarkers { get; init; }

#nullable disable

    [XmlElement("Version")]
    public ObjectVersion[] Versions { get; init; }

    public static ListVersionsResult Deserialize(string xmlText)
    {
        return S3Serializer<ListVersionsResult>.Deserialize(xmlText);
    }
}
