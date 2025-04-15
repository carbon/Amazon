using System.IO;
using System.Net.Http;
using System.Text.Encodings.Web;

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

            var (key, value) = tags[i];

            if (key.Length > 128)
            {
                throw new ArgumentException($"Tag key > 128 chars. Was '{key}'");
            }

            if (value.Length > 256)
            {
                throw new ArgumentException($"Tag value > 256 chars. Was '{value}'");
            }

            UrlEncoder.Default.Encode(writer, key);
            writer.Write('=');
            UrlEncoder.Default.Encode(writer, value);
        }

        Headers.Add(S3HeaderNames.Tagging, writer.ToString());
    }

    public void SetStream(Stream stream, string contentType = "application/octet-stream")
    {
        var hash = stream.CanSeek 
            ? StreamHelper.ComputeSHA256(stream) 
            : null;

        SetStream(stream,
            sha256Hash  : hash, 
            contentType : contentType
        );
    }

    public void SetStream(
        Stream stream,
        byte[]? sha256Hash,
        string contentType = "application/octet-stream")
    {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentException.ThrowIfNullOrEmpty(contentType);

        if (stream.Length is 0)
            throw new ArgumentException("Must not be empty", nameof(stream));

        if (contentType.Length is 0)
            throw new ArgumentException("Required", nameof(contentType));

        Content = new StreamContent(stream) {
            Headers = {
                { "Content-Type", contentType }
            }
        };

        Content.Headers.ContentLength = stream.Length;

        Headers.TryAddWithoutValidation(S3HeaderNames.ContentSha256, sha256Hash is not null
            ? Convert.ToHexStringLower(sha256Hash)
            : "UNSIGNED-PAYLOAD"
        );
    }

    public void SetStream(Stream stream, long length, string contentType = "application/octet-stream")
    {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(length);

        Content = new StreamContent(stream) {
            Headers = {
                { "Content-Type", contentType }
            }
        };

        Content.Headers.ContentLength = length;

        // TODO: Support chunked streaming...

        Headers.TryAddWithoutValidation(S3HeaderNames.ContentSha256, stream.CanSeek
            ? Convert.ToHexStringLower(StreamHelper.ComputeSHA256(stream))
            : "UNSIGNED-PAYLOAD");
    }
}