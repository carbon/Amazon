namespace Amazon.Ssm
{
    public class DocumentIdentifier
    {
      
        public string DocumentType { get; set; }
    
        public string DocumentVersion { get; set; }

        public string Name { get; set; }

        public string Owner { get; set; }

        // Windows | Linux
        public string[] PlatformTypes { get; set; }

        public string SchemaVersion { get; set; }

    }
}
