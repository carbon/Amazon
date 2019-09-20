#nullable disable

namespace Amazon.S3.Events
{
    public class S3EventDetails
    {
        public string S3SchemaVersion { get; set; }

        public string ConfigurationId { get; set; }

        public S3BucketInfo Bucket { get; set; }

        public S3ObjectInfo Object { get; set; }

        public class S3BucketInfo
        {
            public string Name { get; set; }

            public S3UserIdentity OwnerIdentity { get; set; }

            public string Arn { get; set; }
        }

        public class S3ObjectInfo
        {
            public string Key { get; set; }

            public long Size { get; set; }

            public string ETag { get; set; }

            public string VersionId { get; set; }

            public string Sequencer { get; set; }
        }
    }
}
