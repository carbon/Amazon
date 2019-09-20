#nullable disable

using System.Runtime.Serialization;

namespace Amazon.Models
{
    public class AwsError
    {
        [DataMember(Name = "__type")]
        public string Type { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}