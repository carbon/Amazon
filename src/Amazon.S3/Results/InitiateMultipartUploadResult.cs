using System;
using System.IO;
using System.Xml.Serialization;

namespace Amazon.S3
{
    [XmlRoot(Namespace = "http://s3.amazonaws.com/doc/2006-03-01/")]
    public class InitiateMultipartUploadResult : IUpload
    {
        [XmlElement]
        public string Bucket { get; set; }

        [XmlElement]
        public string Key { get; set; }

        [XmlElement]
        public string UploadId { get; set; }

        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(InitiateMultipartUploadResult));

        #region IUpload

        string IUpload.BucketName => Bucket;

        string IUpload.ObjectName => Key;

        string IUpload.Id => UploadId;

        #endregion

        public static InitiateMultipartUploadResult ParseXml(string xmlText)
        {
            #region Preconditions

            if (xmlText == null)
                throw new ArgumentNullException("xmlText");

            #endregion

            using (var reader = new StringReader(xmlText))
            {
                return (InitiateMultipartUploadResult)serializer.Deserialize(reader);
            }
        }
    }

    // TODO: Replace with Carbon.Storage.Uploads.IUpload (1.5)

    public interface IUpload
    {
        string BucketName { get; }

        string ObjectName { get; }

        string Id { get; }
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
