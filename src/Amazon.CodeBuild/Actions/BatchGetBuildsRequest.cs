using System;
using System.ComponentModel.DataAnnotations;

namespace Amazon.CodeBuild;

public sealed class BatchGetBuildsRequest : ICodeBuildRequest
{
    public BatchGetBuildsRequest(params string[] ids)
    {
        ArgumentNullException.ThrowIfNull(ids);

        Ids = ids;
    }

    [Required]
    public string[] Ids { get; }
}