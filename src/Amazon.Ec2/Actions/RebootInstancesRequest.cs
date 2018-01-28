using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Amazon.Ec2
{
    public class RebootInstancesRequest : IEc2Request
    {
        public RebootInstancesRequest() { }

        public RebootInstancesRequest(params string[] instanceIds)
        {
            InstanceIds = instanceIds ?? throw new ArgumentNullException(nameof(instanceIds));
        }

        [DataMember]
        public bool? DryRun { get; set; }

        [DataMember(Name = "InstanceId")]
        public string[] InstanceIds { get; set; }

        public Dictionary<string, string> ToParams()
        {
            return Ec2RequestHelper.ToParams("RebootInstances", this);
        }
    }
}
