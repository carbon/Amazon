using System.Runtime.Serialization;

namespace Amazon.Ssm
{
    public class CommandFilter
    {
        public CommandFilter() { }

        public CommandFilter(string key, string value)
        {
            Key = key;
            Value = value;
        }

        [DataMember(Name = "key")]
        public string Key { get; set; }

        [DataMember(Name = "value")]
        public string Value { get; set; }
    }
}