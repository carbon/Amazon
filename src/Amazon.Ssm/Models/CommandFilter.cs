using System.Runtime.Serialization;

namespace Amazon.Ssm
{
    public class CommandFilter
    {
        [DataMember(Name = "key")]
        public string Key { get; set; }

        [DataMember(Name = "value")]
        public string Value { get; set; }
    }
}