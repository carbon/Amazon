#nullable disable

using System.Xml.Serialization;

namespace Amazon.S3
{
    [XmlRoot("DeleteResult", Namespace = S3Client.Namespace)]
    public sealed class DeleteResult
    {        
        [XmlElement("Deleted")]
        public BatchItem[] Deleted { get; set; }

        [XmlElement("Error")]
        public BatchItemError[] Errors { get; set; }

        #region Helpers

        public bool HasErrors => Errors != null && Errors.Length > 0;

        #endregion

        public static DeleteResult Parse(string xmlText)
        {
            return ResponseHelper<DeleteResult>.ParseXml(xmlText);
        }
    }

    public sealed class BatchItem
    {
        public BatchItem() { }

        public BatchItem(string key)
        {
            Key = key;
        }

        [XmlElement]
        public string Key { get; set; }
    }

    public sealed class BatchItemError
    {
        [XmlElement]
        public string Key { get; set; }

        [XmlElement]
        public string Code { get; set; }

        [XmlElement]
        public string Message { get; set; }
    }
}

/*
<?xml version="1.0" encoding="UTF-8"?>
<DeleteResult xmlns="http://s3.amazonaws.com/doc/2006-03-01/">
	<Deleted>
		<Key>sample1.txt</Key>
	</Deleted>
	<Error>
		<Key>sample2.txt</Key>
		<Code>AccessDenied</Code>
		<Message>Access Denied</Message>
	</Error>
</DeleteResult>
*/
