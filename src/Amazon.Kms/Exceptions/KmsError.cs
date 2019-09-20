#nullable disable

using System.Runtime.Serialization;

namespace Amazon.Kms
{
    public sealed class KmsError
    {
        [DataMember(Name = "__type")]
        public string Type { get; set; }
        
        public string Message { get; set; }
    }
}