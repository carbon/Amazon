using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;

namespace Amazon.S3;

public abstract class S3Request : HttpRequestMessage
{
    internal S3Request(
        HttpMethod method,
        string host,
        string bucketName,
        string? objectName,
        string? versionId = null,
        S3ActionName actionName = default)
    {
        if (host is null)
            throw new ArgumentNullException(nameof(host));

        if (bucketName is null)
            throw new ArgumentNullException(nameof(bucketName));

        BucketName = bucketName;
        ObjectName = objectName;

        // https://{bucket}.s3.amazonaws.com/{key}

        var urlBuilder = new ValueStringBuilder(128);

        urlBuilder.Append("https://");
        urlBuilder.Append(host);
        urlBuilder.Append('/');
        urlBuilder.Append(bucketName);

        // s3.dualstack.{region.Name}.amazonaws.com

        if (objectName is not null)
        {
            urlBuilder.Append('/');
            urlBuilder.Append(objectName);
        }

        if (actionName != default)
        {
            urlBuilder.Append(GetSegment(actionName));
        }

        if (versionId is { Length: > 0 })
        {
            urlBuilder.Append(actionName == default ? '?' : '&');
            urlBuilder.Append("versionId=");
            urlBuilder.Append(versionId);
        }

        RequestUri = new Uri(urlBuilder.ToString());
        Method = method;
    }

    internal S3Request(
       HttpMethod method,
       string host,
       string bucketName,
       Dictionary<string, string> parameters,
       S3ActionName actionName = default)
    {
        if (host is null) throw new ArgumentNullException(nameof(host));

        BucketName = bucketName ?? throw new ArgumentNullException(nameof(bucketName));

        var urlBuilder = new ValueStringBuilder(128);

        urlBuilder.Append("https://");
        urlBuilder.Append(host);
        urlBuilder.Append('/');
        urlBuilder.Append(bucketName);

        int i = 0;

        if (actionName != default)
        {
            urlBuilder.Append(GetSegment(actionName));

            i++;
        }

        if (parameters.Count > 0)
        {
            foreach (var (k, v) in parameters)
            {
                urlBuilder.Append(i is 0 ? '?' : '&');
                urlBuilder.Append(k);

                urlBuilder.Append('=');
                urlBuilder.Append(UrlEncoder.Default.Encode(v));

                i++;
            }
        }

        RequestUri = new Uri(urlBuilder.ToString());
        Method = method;
    }

    private static string GetSegment(S3ActionName action)
    {
        return action switch
        {
            S3ActionName.Tagging => "?tagging",
            S3ActionName.Delete => "?delete",
            S3ActionName.Restore => "?restore",
            S3ActionName.Uploads => "?uploads",
            S3ActionName.Versions => "?versions",
            _ => throw new Exception("Invalid")
        };
    }

    public void SetStorageClass(StorageClass storageClass)
    {
        Headers.Add(S3HeaderNames.StorageClass, storageClass.Name);
    }

    public string BucketName { get; }

    public string? ObjectName { get; }

    public HttpCompletionOption CompletionOption { get; set; } = HttpCompletionOption.ResponseHeadersRead;
}
