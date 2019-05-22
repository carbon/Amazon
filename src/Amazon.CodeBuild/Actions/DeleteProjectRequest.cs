#nullable enable

using System;
using System.ComponentModel.DataAnnotations;

namespace Amazon.CodeBuild
{
    public sealed class DeleteProjectRequest : ICodeBuildRequest
    {
        public DeleteProjectRequest(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        [Required]
        public string Name { get; }
    }
}