using System;

namespace Amazon.Ec2
{
    public /*readonly*/ struct Filter
    {
        public Filter(string name, string value)
        {
            Name = name   ?? throw new ArgumentNullException(nameof(name));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Name { get; }

        public string Value { get; }
    }
}