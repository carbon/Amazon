using System.Xml.Serialization;

namespace Amazon.S3
{
    [XmlRoot(Namespace = "http://s3.amazonaws.com/doc/2006-03-01/")]
    public class CompleteMultipartUploadResult
    {
        [XmlElement]
        public string Location { get; set; }

        [XmlElement]
        public string Bucket { get; set; }

        [XmlElement]
        public string Key { get; set; }

        [XmlElement]
        public string ETag { get; set; }

        public static CompleteMultipartUploadResult ParseXml(string xmlText)
        {
            return ResponseHelper<CompleteMultipartUploadResult>.ParseXml(xmlText);
        }
    }
}

/*
<?xml version="1.0" encoding="UTF-8"?>
<CompleteMultipartUploadResult xmlns="http://s3.amazonaws.com/doc/2006-03-01/">
  <Location>http://Example-Bucket.s3.amazonaws.com/Example-Object</Location>
  <Bucket>Example-Bucket</Bucket>
  <Key>Example-Object</Key>
  <ETag>"3858f62230ac3c915f300c664312c11f-9"</ETag>
</CompleteMultipartUploadResult>
*/
