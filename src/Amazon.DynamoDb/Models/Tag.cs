using System;

namespace Amazon.DynamoDb
{
    public class Tag
    {
        public Tag() { }

        public Tag(string key, string value)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}
