using System.Linq;
using System.Net.Http;

namespace Amazon.S3
{
    public sealed class CopyObjectRequest : S3Request
    {
        public CopyObjectRequest(string host, S3ObjectLocation source, S3ObjectLocation target)
            : base(HttpMethod.Put, host, target.BucketName, target.Key)
        {
            Headers.Add("x-amz-copy-source", $"/{source.BucketName}/{source.Key}");

            CompletionOption = HttpCompletionOption.ResponseContentRead;
        }

        // If undefined, the default is COPY

        public MetadataDirectiveValue? MetadataDirective
        {
            get
            {
                if (this.Headers.TryGetValues(S3HeaderNames.MetadataDirective, out var values))
                {
                    switch (values.FirstOrDefault())
                    {
                        case "COPY": return MetadataDirectiveValue.Copy;
                        case "REPLACE": return MetadataDirectiveValue.Replace;
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
                    _                              => null
                };

                Set(S3HeaderNames.MetadataDirective, val);
            }
        }

        private void Set(string name, string? value)
        {
            if (value is null)
            {
                this.Headers.Remove(name);
            }

            this.Headers.TryAddWithoutValidation(name, value);
        }
    }
}
