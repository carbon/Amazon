namespace Amazon.Ssm
{
    public class ListDocumentVersionsResponse
    {
        public DocumentVersionInfo[] DocumentVersions { get; set; }

        public string NextToken { get; set; }
    }
}