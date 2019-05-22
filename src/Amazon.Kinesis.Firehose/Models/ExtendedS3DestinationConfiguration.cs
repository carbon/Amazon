#nullable disable

using System.Runtime.Serialization;

namespace Amazon.Kinesis.Firehose
{
    public class ExtendedS3DestinationConfiguration
    {
        [DataMember(Name = "BucketARN")]
        public string BucketARN { get; set; }
        
        public BufferingHints BufferingHints { get; set; }

        public CloudWatchLoggingOptions CloudWatchLoggingOptions { get; set; }

        // UNCOMPRESSED | GZIP | ZIP | Snappy
        public string CompressionFormat { get; set; }
        
        public EncryptionConfiguration EncryptionConfiguration { get; set; }

        public string Prefix { get; set; }

        [DataMember(Name = "RoleARN")]
        public string RoleARN { get; set; }
    }
}
