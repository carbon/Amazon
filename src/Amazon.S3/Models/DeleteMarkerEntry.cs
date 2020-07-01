#nullable disable

using System;
using System.Xml.Serialization;

namespace Amazon.S3
{
    public sealed class DeleteMarkerEntry
    {
        [XmlElement("Key")]
        public string Key { get; set; }

        [XmlElement("IsLatest")]
        public bool IsLatest { get; set; }

        [XmlElement("LastModified", DataType = "dateTime")]
        public DateTime LastModified { get; set; }

        [XmlElement("VersionId")]
        public string VersionId { get; set; }
    }
}