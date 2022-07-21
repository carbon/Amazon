﻿using System.ComponentModel.DataAnnotations;

namespace Amazon.CodeBuild;

public sealed class BatchGetProjectsRequest : ICodeBuildRequest
{
    public BatchGetProjectsRequest(params string[] names)
    {
        ArgumentNullException.ThrowIfNull(names);

        Names = names;
    }

    [Required]
    public string[] Names { get; }
}