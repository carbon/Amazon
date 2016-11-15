namespace Amazon.CloudWatch
{
    public class DimensionFilter
    {
        public DimensionFilter(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }

        public string Value { get; }
    }
}
