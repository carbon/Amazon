using System.Collections.Generic;
using System.Xml.Linq;

namespace Amazon.S3
{
    public class DeleteResult
    {
        private static readonly XNamespace ns = S3Client.NS;

        public List<BatchItem> Deleted { get; } = new List<BatchItem>();

        public List<BatchItemError> Errors { get; } = new List<BatchItemError>();

        #region Helpers

        public bool HasErrors => Errors.Count > 0;

        #endregion

        public static DeleteResult Parse(string xmlText)
        {
            var result = new DeleteResult();

            var deleteResultEl = XElement.Parse(xmlText);

            foreach (var el in deleteResultEl.Elements(ns + "Deleted"))
            {
                var deleted = new BatchItem(el.Element(ns + "Key").Value);

                result.Deleted.Add(deleted);
            }

            foreach (var el in deleteResultEl.Elements(ns + "Error"))
            {
                var error = new BatchItemError {
                    Key = el.Element(ns + "Key").Value,
                    Code = el.Element(ns + "Code").Value,
                    Message = el.Element(ns + "Message").Value
                };

                result.Errors.Add(error);
            }

            return result;
        }
    }

    public class BatchItemError
    {
        public string Key { get; set; }

        public string Code { get; set; }

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
