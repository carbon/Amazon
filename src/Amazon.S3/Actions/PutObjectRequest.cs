using System.IO;
using System.Net.Http;
using System.Text.Encodings.Web;

using Amazon.Helpers;

namespace Amazon.S3;

public class PutObjectRequest : S3Request
{
    public PutObjectRequest(string host, string bucketName, string key)
        : base(HttpMethod.Put, host, bucketName, key)
    {
        ArgumentNullException.ThrowIfNull(key);

        CompletionOption = HttpCompletionOption.ResponseContentRead;
    }

    internal void SetCustomerEncryptionKey(in ServerSideEncryptionKey key)
    {
        Headers.Add(S3HeaderNames.ServerSideEncryptionCustomerAlgorithm, key.Algorithm);
        Headers.Add(S3HeaderNames.ServerSideEncryptionCustomerKey, Convert.ToBase64String(key.Key));
        Headers.Add(S3HeaderNames.ServerSideEncryptionCustomerKeyMD5, Convert.ToBase64String(key.KeyMD5));
    }

    internal void SetTagSet(IReadOnlyList<KeyValuePair<string, string>> tags)
    {
        if (tags is null || tags.Count is 0) return;

        if (tags.Count > 10)
        {
            throw new ArgumentException("Must be less than 10", nameof(tags));
        }

        // The tag-set for the object. The tag-set must be encoded as URL Query parameters. (For example, "Key1=Value1")

        using var writer = new StringWriter();

        for (int i = 0; i < tags.Count; i++)
        {
            if (i > 0)
            {
                writer.Write('&');
            }

            KeyValuePair<string, string> tag = tags[i];

            if (tag.Key.Length > 128)
            {
                throw new ArgumentException("Tag key > 128 chars. Was " + tag.Key);
            }

            if (tag.Value.Length > 256)
            {
                throw new ArgumentException("Tag value > 256 chars. Was " + tag.Value);
            }

            UrlEncoder.Default.Encode(writer, tag.Key);
            writer.Write('=');
            UrlEncoder.Default.Encode(writer, tag.Value);
        }

        Headers.Add(S3HeaderNames.Tagging, writer.ToString());
    }

    public void SetStream(Stream stream, string mediaType = "application/octet-stream")
    {
        ArgumentNullException.ThrowIfNull(stream);

        SetStream(stream, sha256Hash: stream.CanSeek ? StreamHelper.ComputeSHA256(stream) : null, mediaType);
    }

    public void SetStream(Stream stream, byte[]? sha256Hash, string mediaType = "application/octet-stream")
    {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentNullException.ThrowIfNull(mediaType);

        if (stream.Length is 0)
            throw new ArgumentException("Must not be empty", nameof(stream));

        if (mediaType.Length is 0)
            throw new ArgumentException("Required", nameof(mediaType));

        Content = new StreamContent(stream)
        {
            Headers = {
                    { "Content-Type", mediaType }
                }
        };

        Content.Headers.ContentLength = stream.Length;

        Headers.Add(S3HeaderNames.ContentSha256, sha256Hash is not null
            ? HexString.FromBytes(sha256Hash)
            : "UNSIGNED-PAYLOAD"
        );
    }

    public void SetStream(Stream stream, long length, string mediaType = "application/octet-stream")
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (length <= 0)
        {
            throw new ArgumentException("Must be greater than 0.", nameof(length));
        }

        Content = new StreamContent(stream)
        {
            Headers = {
                { "Content-Type", mediaType }
            }
        };

        Content.Headers.ContentLength = length;

        // TODO: Support chunked streaming...

        Headers.Add(S3HeaderNames.ContentSha256, stream.CanSeek
            ? HexString.FromBytes(StreamHelper.ComputeSHA256(stream))
            : "UNSIGNED-PAYLOAD");
    }
}
