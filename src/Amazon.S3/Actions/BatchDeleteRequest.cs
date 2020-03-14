using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace Amazon.S3
{
    public sealed class BatchDeleteRequest : S3Request
    {
        public BatchDeleteRequest(string host, string bucketName, DeleteBatch batch)
            : base(HttpMethod.Post, host, bucketName, "?delete")
        {
            var xmlText = batch.ToXmlString();

            Content = new StringContent(xmlText, Encoding.UTF8, "text/xml");

            Content.Headers.ContentMD5 = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(xmlText));

            CompletionOption = HttpCompletionOption.ResponseContentRead;
        }
    }

    public readonly struct DeleteBatch
    {
        private readonly string[] keys;

        public DeleteBatch(params string[] keys)
        {
            this.keys = keys ?? throw new ArgumentNullException(nameof(keys));
        }

        public string ToXmlString()
        {
            var root = new XElement("Delete");

            foreach (string key in keys)
            {
                root.Add(new XElement("Object",
                    new XElement("Key", key)
                ));
            }

            return root.ToString();
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
