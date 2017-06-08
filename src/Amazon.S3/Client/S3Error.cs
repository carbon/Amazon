using System;
using System.IO;
using System.Xml.Serialization;

namespace Amazon.S3
{
    [XmlRoot("Error")]
    public class S3Error
    {
        [XmlElement]
        public string Code { get; set; }

        [XmlElement]
        public string Message { get; set; }

        [XmlElement]
        public string Resource { get; set; }

        [XmlElement]
        public string RequestId { get; set; }

        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(S3Error));

        public static S3Error ParseXml(string xmlText)
        {
            #region Preconditions

            if (xmlText == null) throw new ArgumentNullException("xmlText");

            #endregion

            try
            {
                using (var reader = new StringReader(xmlText))
                {
                    return (S3Error)serializer.Deserialize(reader);
                }
            }
            catch
            {
                throw new Exception(xmlText);
            }
        }
    }
}

/*
<?xml version="1.0" encoding="UTF-8"?>
<Error>
	<Code>NoSuchKey</Code>
	<Message>The resource you requested does not exist</Message>
	<Resource>/mybucket/myfoto.jpg</Resource> 
	<RequestId>4442587FB7D0A2F9</RequestId>
 	<HostId>4PsK3Ki9G28+pJeh0c3jo3V2sqnftQ5DROhs+U9p4SaJk4BHmjvB2xZfDUgIuENf</HostId>
</Error>
*/