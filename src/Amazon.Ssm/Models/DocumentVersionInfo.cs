namespace Amazon.Ssm
{
    public class DocumentVersionInfo
    {
        public string CreatedDate { get; set; }

        public string DocumentVersion { get; set; }

        public bool IsDefaultVersion { get; set; }

        public string Name { get; set; }
    }
}
