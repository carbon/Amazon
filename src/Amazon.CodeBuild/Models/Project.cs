#nullable disable

namespace Amazon.CodeBuild
{
    public class Project
    {
        public string Name { get; set; }

        public string Arn { get; set; }

        public ProjectArtifacts[] Artifacts { get; set; }

        public string Description { get; set; }

        public string EncryptionKey { get; set; }

        public ProjectEnvironment Environment { get; set; }

        public Timestamp Created { get; set; }

        public Timestamp LastModified { get; set; }

        public string ServiceRole { get; set; }
        
        public Tag[] Tags { get; set; }

        public int TimeoutInMinutes { get; set; }
    }
}