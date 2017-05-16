using System.ComponentModel.DataAnnotations;

namespace Amazon.CodeBuild
{
    public class ListBuildsForProjectRequest : ICodeBuildRequest
    {
        public string NextToken { get; set; }

        [Required]
        public string ProjectName { get; set; }

        // ASCENDING | DESCENDING
        public string SortOrder { get; set; }
    }
}