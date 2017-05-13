namespace Amazon.Ec2
{
    public class Tag
    {
        public Tag() { }

        public Tag(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}
