using System.Net.Http;

namespace Amazon.S3;

public sealed class ObjectHeadRequest(string host, string bucketName, string key) 
    : S3Request(HttpMethod.Head, host, bucketName, key)
{
    internal void SetCustomerEncryptionKey(in ServerSideEncryptionKey key)
    {
        Headers.Add(S3HeaderNames.ServerSideEncryptionCustomerAlgorithm, key.Algorithm);
        Headers.Add(S3HeaderNames.ServerSideEncryptionCustomerKey, Convert.ToBase64String(key.Key));
        Headers.Add(S3HeaderNames.ServerSideEncryptionCustomerKeyMD5, Convert.ToBase64String(key.KeyMD5));
    }
}