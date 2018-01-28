using System.ComponentModel.DataAnnotations;

namespace Amazon.CodeBuild
{
    public class ProjectArtifacts
    {
        public string Location { get; set; }

        public string Name { get; set; }

        // NONE | BUILD_ID
        public string NamespaceType { get; set; }

        public string Packaging { get; set; }

        public string Path { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
