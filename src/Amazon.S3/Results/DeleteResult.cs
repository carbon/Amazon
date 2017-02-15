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
                var error = new BatchItemError(
                    key     : el.Element(ns + "Key").Value,
                    code    : el.Element(ns + "Code").Value,
                    message : el.Element(ns + "Message").Value
                );

                result.Errors.Add(error);
            }

            return result;
        }
    }

    public class BatchItemError
    {
        public BatchItemError(string key, string code, string message)
        {
            Key = key;
            Code = code;
            Message = message;
        }

        public string Key { get; }

        public string Code { get; }

        public string Message { get; }
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
