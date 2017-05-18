using System;
using System.ComponentModel.DataAnnotations;

namespace Amazon.CodeBuild
{
    public class BatchGetBuildsRequest : ICodeBuildRequest
    {
        public BatchGetBuildsRequest() { }

        public BatchGetBuildsRequest(params string[] ids)
        {
            Ids = ids ?? throw new ArgumentNullException(nameof(ids));
        }

        [Required]
        public string[] Ids { get; set; }
    }
}