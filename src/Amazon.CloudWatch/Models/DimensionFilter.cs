namespace Amazon.CloudWatch
{
    public readonly struct DimensionFilter
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
