using System.Net.Http;

namespace Amazon.S3;

public sealed class CopyObjectRequest : S3Request
{
    public CopyObjectRequest(string host, S3ObjectLocation source, S3ObjectLocation destination)
        : base(HttpMethod.Put, host, destination.BucketName, destination.Key)
    {
        Headers.Add(S3HeaderNames.CopySource, $"/{source.BucketName}/{source.Key}");

        CompletionOption = HttpCompletionOption.ResponseContentRead;
    }

    // If undefined, the default is COPY

    public MetadataDirectiveValue? MetadataDirective
    {
        get
        {
            if (Headers.NonValidated.TryGetValues(S3HeaderNames.MetadataDirective, out var value))
            {
                switch (value.ToString())
                {
                    case "COPY"    : return MetadataDirectiveValue.Copy;
                    case "REPLACE" : return MetadataDirectiveValue.Replace;
                }
            }

            return null;
        }

        set
        {
            string? val = value switch
            {
                MetadataDirectiveValue.Copy    => "COPY",
                MetadataDirectiveValue.Replace => "REPLACE",
                _ => null
            };

            Set(S3HeaderNames.MetadataDirective, val);
        }
    }

    private void Set(string name, string? value)
    {
        if (value is null)
        {
            Headers.Remove(name);
        }
        else
        {
            Headers.TryAddWithoutValidation(name, value);
        }
    }
}