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
                if (this.Headers.TryGetValues("x-amz-metadata-directive", out var values))
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
                string val = null;

                switch (value)
                {
                    case MetadataDirectiveValue.Copy:
                        val = "COPY";
                        break;
                    case MetadataDirectiveValue.Replace:
                        val = "REPLACE";
                        break;
                }

                Set("x-amz-metadata-directive", val);
            }
        }


        private void Set(string name, string value)
        {
            if (value is null)
            {
                this.Headers.Remove(name);
            }

            this.Headers.TryAddWithoutValidation(name, value);
        }

        /*
        x-amz-metadata-directive: metadata_directive
        x-amz-copy-source-if-match: etag
        x-amz-copy-source-if-none-match: etag
        x-amz-copy-source-if-unmodified-since: time_stamp
        x-amz-copy-source-if-modified-since: time_stamp
        */

    }


    public enum MetadataDirectiveValue
    {
        Copy = 1,
        Replace = 2
    }
}
