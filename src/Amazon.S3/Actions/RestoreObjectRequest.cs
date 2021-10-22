using System.Net.Http;
using System.Text;

namespace Amazon.S3;

public sealed class RestoreObjectRequest : S3Request
{
    public RestoreObjectRequest(string host, string bucketName, string key, string? version = null)
        : base(HttpMethod.Post, host, bucketName, key, version, S3ActionName.Restore)
    {
        ArgumentNullException.ThrowIfNull(key);

        string xmlText = GetXmlString();

        Content = new StringContent(xmlText, Encoding.UTF8, "text/xml");

        Content.Headers.ContentMD5 = HashHelper.ComputeMD5Hash(xmlText);

        CompletionOption = HttpCompletionOption.ResponseContentRead;
    }

    public GlacierJobTier Tier { get; init; } = GlacierJobTier.Standard;

    public int Days { get; init; } = 7; // Default to 7

    public string GetXmlString() =>
FormattableString.Invariant($@"<RestoreRequest>
  <Days>{Days}</Days>
  <GlacierJobParameters>
    <Tier>{Tier}</Tier>
  </GlacierJobParameters>
</RestoreRequest>");
}

/*
POST /ObjectName?restore&versionId=VersionID HTTP/1.1
Host: BucketName.s3.amazonaws.com
<RestoreRequest xmlns="http://s3.amazonaws.com/doc/2006-3-01">
   <Days>NumberOfDays</Days>
</RestoreRequest> 
*/
