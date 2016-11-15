using System;
using System.Xml.Linq;

namespace Amazon.S3
{
    public class CopyObjectResult
    {
        private static readonly XNamespace ns = "http://s3.amazonaws.com/doc/2006-03-01/";

        public DateTime LastModified { get; set; }

        public string ETag { get; set; }

        public static CopyObjectResult ParseXml(string xmlText)
        {
            #region Preconditions

            if (xmlText == null) throw new ArgumentNullException(nameof(xmlText));

            #endregion

            var copyObjectResultEl = XElement.Parse(xmlText);

            return new CopyObjectResult {
                LastModified = DateTime.Parse(copyObjectResultEl.Element(ns + "LastModified").Value),
                ETag = copyObjectResultEl.Element(ns + "ETag").Value
            };
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
