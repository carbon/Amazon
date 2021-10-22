#nullable disable

using System;
using System.ComponentModel.DataAnnotations;

namespace Amazon.CodeBuild
{
    public sealed class EnvironmentVariable
    {
        public EnvironmentVariable() { }

        public EnvironmentVariable(string name, string value)
        {
            ArgumentNullException.ThrowIfNull(name);
            ArgumentNullException.ThrowIfNull(value);

            Name = name;
            Value = value;
        }

        [Required]
        public string Name { get; init; }

        [Required]
        public string Value { get; init; }
    }
}
