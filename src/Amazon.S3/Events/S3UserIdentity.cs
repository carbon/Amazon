#nullable disable

using System.Runtime.Serialization;

namespace Amazon.S3.Events
{
    public class S3UserIdentity
    {
        [DataMember(Name = "principalId")]
        public string PrincipalId { get; set; }
    }
}