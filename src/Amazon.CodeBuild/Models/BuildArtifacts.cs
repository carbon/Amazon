#nullable disable

namespace Amazon.CodeBuild
{
    public class BuildArtifacts
    {
        public string Location { get; set; }

        public string Md5Sum { get; set; }

        public string Sha256Sum { get; set; }
    }
}
