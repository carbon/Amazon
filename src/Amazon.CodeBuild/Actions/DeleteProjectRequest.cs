using System;
using System.ComponentModel.DataAnnotations;

namespace Amazon.CodeBuild;

public sealed class DeleteProjectRequest : ICodeBuildRequest
{
    public DeleteProjectRequest(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        Name = name;
    }

    [Required]
    public string Name { get; }
}
