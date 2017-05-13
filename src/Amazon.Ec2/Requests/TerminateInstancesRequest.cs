using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Amazon.Ec2
{
    public class TerminateInstancesRequest : IEc2Request
    {
        [DataMember]
        public bool? DryRun { get; set; }

        [DataMember(Name = "InstanceId")]
        public string[] InstanceIds { get; set; }

        public Dictionary<string, string> ToParams()
        {
            return Ec2RequestHelper.ToParams("TerminateInstances", this);
        }
    }
}