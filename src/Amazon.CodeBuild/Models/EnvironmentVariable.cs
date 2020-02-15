#nullable disable

using System;
using System.ComponentModel.DataAnnotations;

namespace Amazon.CodeBuild
{
    public class EnvironmentVariable
    {
        public EnvironmentVariable() { }

        public EnvironmentVariable(string name, string value)
        {
            Name  = name  ?? throw new ArgumentNullException(nameof(name));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
