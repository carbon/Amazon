#nullable disable

namespace Amazon.Ssm
{
    public sealed class DocumentVersionInfo
    {
        public string Name { get; set; }

        public Timestamp CreatedDate { get; set; }

        public string DocumentVersion { get; set; }

        public bool IsDefaultVersion { get; set; }
    }
}