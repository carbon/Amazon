namespace Amazon.CloudWatch
{
    public class Dimension
    {
        public Dimension(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }

        public string Value { get; }
    }
}
