#nullable disable

namespace Amazon.CodeBuild
{
    public class Tag
    {
        public Tag() { }

        public Tag(string key, string value)
        {
            Key   = key;
            Value = value;
        }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}