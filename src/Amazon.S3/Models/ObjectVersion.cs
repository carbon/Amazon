#nullable disable

using System;
using System.Xml.Serialization;

namespace Amazon.S3
{
    public sealed class ObjectVersion
    {
        [XmlElement("Key")]
        public string Key { get; set; }

        [XmlElement("ETag")]
        public string ETag { get; set; }

        [XmlElement("IsLatest")]
        public bool IsLatest { get; set; }

        [XmlElement("LastModified", DataType = "dateTime")]
        public DateTime LastModified { get; set; }

        [XmlElement("Size")]
        public long Size { get; set; }

        [XmlElement("StorageClass")]
        public string StorageClass { get; set; }

        [XmlElement("VersionId")]
        public string VersionId { get; set; }

        [XmlElement("Owner")]
        public Owner Owner { get; set; }
    }
}