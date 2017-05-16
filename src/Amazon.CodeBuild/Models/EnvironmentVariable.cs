
using System.ComponentModel.DataAnnotations;

namespace Amazon.CodeBuild
{
    public class EnvironmentVariable
    {
        public EnvironmentVariable() { }

        public EnvironmentVariable(string name, string value)
        {
            Name = name;
            Value = value;
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
