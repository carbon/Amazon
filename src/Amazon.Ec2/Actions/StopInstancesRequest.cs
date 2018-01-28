using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Amazon.Ec2
{
    public class StopInstancesRequest : IEc2Request
    {
        public StopInstancesRequest() { }

        public StopInstancesRequest(params string[] instanceIds)
        {
            InstanceIds = instanceIds ?? throw new ArgumentNullException(nameof(instanceIds));
        }

        [DataMember]
        public bool? DryRun { get; set; }

        [DataMember]
        public bool? Force { get; set; }

        [DataMember(Name = "InstanceId")]
        public string[] InstanceIds { get; set; }

        public Dictionary<string, string> ToParams()
        {
            return Ec2RequestHelper.ToParams("StopInstances", this);
        }
    }
}
