#nullable disable

namespace Amazon.Ssm
{
    public sealed class Parameter
    {
        public Parameter() { }

        public Parameter(string name, string type, string value)
        {
            Name  = name;
            Type  = type;
            Value = value;
        }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }
    }
}