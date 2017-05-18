using System;
using System.ComponentModel.DataAnnotations;

namespace Amazon.CodeBuild
{
    public class DeleteProjectRequest : ICodeBuildRequest
    {
        public DeleteProjectRequest() { }

        public DeleteProjectRequest(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        [Required]
        public string Name { get; set; }
    }
}