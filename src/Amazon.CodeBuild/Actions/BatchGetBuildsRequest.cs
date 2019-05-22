#nullable enable

using System;
using System.ComponentModel.DataAnnotations;

namespace Amazon.CodeBuild
{
    public sealed class BatchGetBuildsRequest : ICodeBuildRequest
    {
        public BatchGetBuildsRequest(params string[] ids)
        {
            Ids = ids ?? throw new ArgumentNullException(nameof(ids));
        }

        [Required]
        public string[] Ids { get; }
    }
}