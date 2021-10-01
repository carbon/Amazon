using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace Amazon.S3;

public sealed class PutObjectTaggingRequest : S3Request
{
    public PutObjectTaggingRequest(
        string host, 
        string bucketName, 
        string objectName,
        string? versionId, 
        IEnumerable<KeyValuePair<string, string>> tagset)
        : base(HttpMethod.Put, host, bucketName, objectName, versionId, actionName: S3ActionName.Tagging)
    {
        string xmlText = ToXmlString(tagset);

        byte[] data = Encoding.UTF8.GetBytes(xmlText);

        Content = new ByteArrayContent(data) {
            Headers = { { "Content-Type", "text/xml" } }
        };
            
        Content.Headers.ContentMD5 = MD5.HashData(data);

        CompletionOption = HttpCompletionOption.ResponseContentRead;
    }

    private static string ToXmlString(IEnumerable<KeyValuePair<string, string>> tagset)
    {
        var root = new XElement("Tagging");

        var tagSet = new XElement("TagSet");

        foreach (var tag in tagset)
        {
            tagSet.Add(new XElement("Tag",
                new XElement("Key", tag.Key),
                new XElement("Value", tag.Value)
            ));
        }

        root.Add(tagSet);

        return root.ToString(SaveOptions.DisableFormatting);
    }
}

/*
<Tagging>
   <TagSet>
      <Tag>
         <Key>string</Key>
         <Value>string</Value>
      </Tag>
   </TagSet>
</Tagging>
*/