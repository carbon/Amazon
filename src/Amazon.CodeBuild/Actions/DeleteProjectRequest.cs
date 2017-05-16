using System.ComponentModel.DataAnnotations;

namespace Amazon.CodeBuild
{
    public class DeleteProjectRequest : ICodeBuildRequest
    {
        public DeleteProjectRequest() { }

        public DeleteProjectRequest(string name)
        {
            Name = name;
        }

        [Required]
        public string Name { get; set; }
    }
}