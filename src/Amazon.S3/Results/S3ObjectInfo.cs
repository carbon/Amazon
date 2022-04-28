using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

using Carbon.Storage;

namespace Amazon.S3;

public sealed class S3ObjectInfo : IBlob
{
    public S3ObjectInfo(string key, long size)
    {
        ArgumentNullException.ThrowIfNull(key);

        Key = key;
        ContentLength = size;
    }

    public S3ObjectInfo(string key, long size, DateTime modified)
    {
        ArgumentNullException.ThrowIfNull(key);

        Key = key;
        ContentLength = size;
        Modified = modified;
    }

    internal S3ObjectInfo(
        string bucketName,
        string key,
        long contentLength,
        DateTime modified,
        ETag eTag,
        Dictionary<string, string> properties)
    {
        BucketName = bucketName;
        Key = key;
        ContentLength = contentLength;
        Modified = modified;
        ETag = eTag;
        Properties = properties;
    }

    public string? BucketName { get; }

    public string Key { get; }

    public ETag ETag { get; }

    public long ContentLength { get; }

    public DateTime Modified { get; }

#nullable disable
    public IReadOnlyDictionary<string, string> Properties { get; }
#nullable enable

    #region IBlob

    long IBlob.Size => ContentLength;

    ValueTask<Stream> IBlob.OpenAsync()
    {
        throw new NotImplementedException();
    }

    void IDisposable.Dispose() { }

    #endregion

    #region Helpers

    internal static S3ObjectInfo FromResponse(
        string bucketName, 
        string objectKey, 
        HttpResponseMessage response)
    {
        ETag eTag = default;

        if (response.Headers.ETag is EntityTagHeaderValue et)
        {
            eTag = new ETag(et.Tag);
        }

        var properties = response.GetProperties();

        long contentLength = long.Parse(properties["Content-Length"], NumberStyles.None, CultureInfo.InvariantCulture);

        DateTimeOffset lastModified = DateTimeOffset.ParseExact(properties["Last-Modified"], "R", CultureInfo.InvariantCulture);

        return new S3ObjectInfo(
            bucketName    : bucketName,
            key           : objectKey,
            contentLength : contentLength,
            modified      : lastModified.UtcDateTime, // Last-Modified
            eTag          : eTag,
            properties    : properties
        );
    }

    public string? ContentType => Properties?["Content-Type"];

    public string? VersionId
    {
        get
        {
            if (Properties is null) return null;

            return Properties.TryGetValue(S3HeaderNames.VersionId, out string? version) ? version : null;
        }
    }

    public string? StorageClass => Properties?[S3HeaderNames.StorageClass];

    #endregion
}
