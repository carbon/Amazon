#nullable disable

namespace Amazon.Ssm
{
    public sealed class GetDocumentResponse
    {
        public string Content { get; set; }

        public string DocumentType { get; set; }

        public string DocumentVersion { get; set; }

        public string Name { get; set; }
    }
}