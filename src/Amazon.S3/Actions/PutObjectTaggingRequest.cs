using System.Net.Http;
using System.Security.Cryptography;

using Amazon.S3.Helpers;

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
        byte[] data = ToXmlUtf8Bytes(tagset);

        Content = new ByteArrayContent(data) {
            Headers = { { "Content-Type", "text/xml" } }
        };
            
        Content.Headers.ContentMD5 = MD5.HashData(data);

        CompletionOption = HttpCompletionOption.ResponseContentRead;
    }

    private static byte[] ToXmlUtf8Bytes(IEnumerable<KeyValuePair<string, string>> tagSet)
    {
        var writer = new XmlStringBuilder(pretty: false);

        writer.WriteTagStart("Tagging");
        {
            writer.WriteTagStart("TagSet");
            {
                foreach (var tag in tagSet)
                {
                    writer.WriteTagStart("Tag");
                    {
                        writer.WriteTag("Key", tag.Key);
                        writer.WriteTag("Value", tag.Value);
                    }

                    writer.WriteTagEnd("Tag");
                }
            }
            writer.WriteTagEnd("TagSet");
        }
        writer.WriteTagEnd("Tagging");

        return writer.ToUtf8Bytes();
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