using System.ComponentModel.DataAnnotations;

namespace Amazon.CodeBuild
{
    public class BatchGetBuildsRequest : ICodeBuildRequest
    {
        [Required]
        public string[] Ids { get; set; }
    }
}