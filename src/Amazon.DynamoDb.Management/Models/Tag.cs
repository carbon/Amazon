using System;

namespace Amazon.DynamoDb.Models
{
    public sealed class Tag
    {
#nullable disable
        public Tag() { }
#nullable enable

        public Tag(string key, string value)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}
