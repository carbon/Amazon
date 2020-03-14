#nullable disable

namespace Amazon.S3.Events
{
    public sealed class S3EventDetails
    {
        public string S3SchemaVersion { get; set; }

        public string ConfigurationId { get; set; }

        public S3EventBucketInfo Bucket { get; set; }

        public S3EventObjectInfo Object { get; set; } 
    }
}
