using System.Globalization;
using System.Net.Http;
using System.Text;

namespace Amazon.S3;

public sealed class RestoreObjectRequest : S3Request
{
    public RestoreObjectRequest(string host, string bucketName, string key, string? version = null)
        : base(HttpMethod.Post, host, bucketName, key, version, S3ActionName.Restore)
    {
        ArgumentNullException.ThrowIfNull(key);

        SetContent();

        CompletionOption = HttpCompletionOption.ResponseContentRead;
    }

    public GlacierJobTier Tier
    {
        get => _tier;
        init
        {
            _tier = value;

            SetContent();
        }
    }

    public int Days
    {
        get => _days;
        init
        {
            _days = value;

            SetContent();
        }
    }

    private readonly GlacierJobTier _tier = GlacierJobTier.Standard;
    private readonly int _days = 7; // Default to 7

    // Initializers run after the constructor, so the content is rebuilt whenever they change a value
    private void SetContent()
    {
        string xmlText = GetXmlString();

        Content = new StringContent(xmlText, Encoding.UTF8, "text/xml");

        Content.Headers.ContentMD5 = HashHelper.ComputeMD5Hash(xmlText);
    }

    public string GetXmlString()
    {
        return string.Create(CultureInfo.InvariantCulture, 
            $"""
            <RestoreRequest>
              <Days>{Days}</Days>
              <GlacierJobParameters>
                <Tier>{Tier}</Tier>
              </GlacierJobParameters>
            </RestoreRequest>
            """
        );
    }
}

/*
POST /ObjectName?restore&versionId=VersionID HTTP/1.1
Host: BucketName.s3.amazonaws.com
<RestoreRequest xmlns="http://s3.amazonaws.com/doc/2006-3-01">
   <Days>NumberOfDays</Days>
</RestoreRequest> 
*/
