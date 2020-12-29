﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;

namespace Amazon.S3
{
    public sealed class DeleteObjectsRequest : S3Request
    {
        public DeleteObjectsRequest(string host, string bucketName, DeleteBatch batch)
            : base(HttpMethod.Post, host, bucketName, objectName: null, actionName: S3ActionName.Delete)
        {
            string xmlText = batch.ToXmlString();

            byte[] data = Encoding.UTF8.GetBytes(xmlText);

            Content = new ByteArrayContent(data) {
                Headers = { { "Content-Type", "text/xml" } }
            };

            Content.Headers.ContentMD5 = HashHelper.ComputeMD5Hash(data);

            CompletionOption = HttpCompletionOption.ResponseContentRead;
        }
    }

    public sealed class DeleteBatch
    {
        private readonly IReadOnlyList<string> keys;

        public DeleteBatch(IReadOnlyList<string> keys, bool quite = false)
        {
            if (keys is null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            if (keys.Count > 1_000)
            {
                throw new ArgumentException($"May not exceed 1,000 items. Was {keys.Count} items.", nameof(keys));
            }

            this.keys = keys;
            Quite = quite;
        }

        public bool Quite { get; }

        public string ToXmlString(SaveOptions options = SaveOptions.DisableFormatting)
        {
            var root = new XElement("Delete");

            if (Quite)
            {
                root.Add(new XElement("Quiet", true));
            }

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