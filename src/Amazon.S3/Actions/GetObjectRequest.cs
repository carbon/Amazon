using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Amazon.S3;

public sealed class GetObjectRequest : S3Request
{
    public GetObjectRequest(string host, string bucketName, string key)
        : base(HttpMethod.Get, host, bucketName, key)
    {
        if (key is null) throw new ArgumentNullException(nameof(key));

        CompletionOption = HttpCompletionOption.ResponseHeadersRead;
    }

    public DateTimeOffset? IfModifiedSince
    {
        get => Headers.IfModifiedSince;
        set => Headers.IfModifiedSince = value;
    }

    public string IfNoneMatch
    {
        set
        {
            if (value is null)
            {
                Headers.IfNoneMatch.Clear();
            }
            else
            {
                Headers.IfNoneMatch.Add(new EntityTagHeaderValue(value));
            }
        }
    }

    internal void SetCustomerEncryptionKey(in ServerSideEncryptionKey key)
    {
        Headers.Add(S3HeaderNames.ServerSideEncryptionCustomerAlgorithm, key.Algorithm);
        Headers.Add(S3HeaderNames.ServerSideEncryptionCustomerKey, Convert.ToBase64String(key.Key));
        Headers.Add(S3HeaderNames.ServerSideEncryptionCustomerKeyMD5, Convert.ToBase64String(key.KeyMD5));
    }

    public void SetRange(long? from, long? to)
    {
        Headers.Range = new RangeHeaderValue(from, to);
    }
}
