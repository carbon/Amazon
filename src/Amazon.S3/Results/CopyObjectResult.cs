#nullable disable

using System;
using System.Xml.Serialization;

namespace Amazon.S3
{
    [XmlRoot(Namespace = S3Client.Namespace)]
    public sealed class CopyObjectResult
    {
        [XmlElement(DataType = "dateTime")]
        public DateTime LastModified { get; set; }

        [XmlElement]
        public string ETag { get; set; }

        public static CopyObjectResult ParseXml(string xmlText)
        {
            return ResponseHelper<CopyObjectResult>.ParseXml(xmlText);
        }
    }
}

/*	
<CopyObjectResult xmlns="http://s3.amazonaws.com/doc/2006-03-01/">
	<LastModified>2008-02-20T22:13:01</LastModified>
	<ETag>"7e9c608af58950deeb370c98608ed097"</ETag>
</CopyObjectResult> 
  
Returns the ETag of the new object. The ETag only reflects changes to the contents of an object, not its metadata.
*/