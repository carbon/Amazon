#nullable disable

using System.Xml.Serialization;

namespace Amazon.S3
{
    [XmlRoot("ListVersionsResult", Namespace = S3Client.Namespace)]
    public sealed class ListVersionsResult
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("KeyMarker")]
        public string KeyMarker { get; set; }

        [XmlElement("MaxKeys")]
        public int MaxKeys { get; set; }

        [XmlElement("Prefix")]
        public string Prefix { get; set; }

        [XmlElement("VersionIdMarker")]
        public string VersionIdMarker { get; set; }

        [XmlElement("IsTruncated")]
        public bool IsTruncated { get; set; }

#nullable enable
        [XmlElement("DeleteMarker")]
        public DeleteMarkerEntry[]? DeleteMarkers { get; set; }

#nullable disable

        [XmlElement("Version")]
        public ObjectVersion[] Versions { get; set; }

		public static ListVersionsResult ParseXml(string xmlText)
        {
            return ResponseHelper<ListVersionsResult>.ParseXml(xmlText);
        }
    }
}
