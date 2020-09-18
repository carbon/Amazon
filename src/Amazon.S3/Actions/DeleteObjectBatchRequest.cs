using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace Amazon.S3
{
    public sealed class DeleteObjectBatchRequest : S3Request
    {
        public DeleteObjectBatchRequest(string host, string bucketName, DeleteBatch batch)
            : base(HttpMethod.Post, host, bucketName, objectName: null, actionName: S3ActionName.Delete)
        {
            string xmlText = batch.ToXmlString();

            byte[] data = Encoding.UTF8.GetBytes(xmlText);

            Content = new ByteArrayContent(data) {
                Headers = { { "Content-Type", "text/xml" } }
            };

            using MD5 md5 = MD5.Create();

            Content.Headers.ContentMD5 = md5.ComputeHash(data);

            CompletionOption = HttpCompletionOption.ResponseContentRead;
        }
    }

    public sealed class DeleteBatch
    {
        private readonly IReadOnlyList<string> keys;

        public DeleteBatch(IReadOnlyList<string> keys)
        {
            if (keys is null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            if (keys.Count > 1000)
            {
                throw new ArgumentException($"May not exceed 1000 items. Was {keys.Count} items.", nameof(keys));
            }

            this.keys = keys;
        }

        public string ToXmlString(SaveOptions options = SaveOptions.DisableFormatting)
        {
            var root = new XElement("Delete");

            foreach (string key in keys)
            {
                root.Add(new XElement("Object",
                    new XElement("Key", key)
                ));
            }

            return root.ToString(options);
        }
    }
}

/*
<?xml version="1.0" encoding="UTF-8"?>
<Delete>
    <Quiet>true</Quiet>
    <Object>
         <Key>Key</Key>
         <VersionId>VersionId</VersionId>
    </Object>
    <Object>
         <Key>Key</Key>
    </Object>
    ...
</Delete>	
*/
