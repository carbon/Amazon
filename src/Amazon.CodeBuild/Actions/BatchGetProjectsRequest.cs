using System.ComponentModel.DataAnnotations;

namespace Amazon.CodeBuild
{
    public class BatchGetProjectsRequest : ICodeBuildRequest
    {
        [Required]
        public string[] Names { get; set; }
    }
}