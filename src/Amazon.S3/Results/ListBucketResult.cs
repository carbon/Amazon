#nullable disable

using System.Xml.Serialization;

namespace Amazon.S3;

[XmlRoot("ListBucketResult", Namespace = S3Client.Namespace)]
public sealed class ListBucketResult
{
    [XmlElement("Name")]
    public string Name { get; init; }

#nullable enable

    /// <summary>
    /// If StartAfter was sent with the request, it is included in the response.
    /// </summary>
    [XmlElement("StartAfter")]
    public string? StartAfter { get; init; }

    [XmlElement("KeyCount")]
    public int KeyCount { get; init; }

    [XmlElement("MaxKeys")]
    public int MaxKeys { get; init; }

    [XmlElement("Prefix")]
    public string? Prefix { get; init; }

    [XmlElement("NextContinuationToken")]
    public string? NextContinuationToken { get; init; }

    [XmlElement("IsTruncated")]
    public bool IsTruncated { get; init; }

#nullable disable

    [XmlElement("Contents")]
    public ListBucketObject[] Items { get; init; }

    public static ListBucketResult Deserialize(string xmlText)
    {
        return S3Serializer<ListBucketResult>.Deserialize(xmlText);
    }
}