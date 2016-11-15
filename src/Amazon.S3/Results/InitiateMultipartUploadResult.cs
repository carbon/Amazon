using System;
using System.IO;
using System.Xml.Serialization;

namespace Amazon.S3
{
    [XmlRoot(Namespace = "http://s3.amazonaws.com/doc/2006-03-01/")]
    public class InitiateMultipartUploadResult
    {
        [XmlElement]
        public string Bucket { get; set; }

        [XmlElement]
        public string Key { get; set; }

        [XmlElement]
        public string UploadId { get; set; }

        public static InitiateMultipartUploadResult ParseXml(string xmlText)
        {
            #region Preconditions

            if (xmlText == null)
                throw new ArgumentNullException("xmlText");

            #endregion

            using (var reader = new StringReader(xmlText))
            {
                var serializer = new XmlSerializer(typeof(InitiateMultipartUploadResult));

                return (InitiateMultipartUploadResult)serializer.Deserialize(reader);
            }
        }
    }
}

/*
<?xml version="1.0" encoding="UTF-8"?>
<InitiateMultipartUploadResult xmlns="http://s3.amazonaws.com/doc/2006-03-01/">
  <Bucket>example-bucket</Bucket>
  <Key>example-object</Key>
  <UploadId>VXBsb2FkIElEIGZvciA2aWWpbmcncyBteS1tb3ZpZS5tMnRzIHVwbG9hZA</UploadId>
</InitiateMultipartUploadResult>
*/
