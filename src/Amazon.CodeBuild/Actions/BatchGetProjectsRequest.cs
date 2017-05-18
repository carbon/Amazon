using System.ComponentModel.DataAnnotations;

namespace Amazon.CodeBuild
{
    public class BatchGetProjectsRequest : ICodeBuildRequest
    {
        public BatchGetProjectsRequest() { }

        public BatchGetProjectsRequest(params string[] names)
        {
            Names = names;
        }

        [Required]
        public string[] Names { get; set; }
    }
}