#nullable disable

namespace Amazon.S3.Events
{
    public sealed class S3EventBucketInfo
    {
        public string Name { get; set; }

        public S3UserIdentity OwnerIdentity { get; set; }

        public string Arn { get; set; }
    }
}