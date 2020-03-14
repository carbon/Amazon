#nullable disable

namespace Amazon.S3.Events
{
    public sealed class S3EventObjectInfo
    {
        public string Key { get; set; }

        public long Size { get; set; }

        public string ETag { get; set; }

        public string VersionId { get; set; }

        public string Sequencer { get; set; }
    }
}