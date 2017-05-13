using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

using Carbon.Json;

namespace Amazon.Ec2
{
    public class RunInstancesRequest : IEc2Request
    {
        [DataMember(Name = "BlockDeviceMapping")]
        public List<BlockDeviceMapping> BlockDeviceMappings { get; set; }

        // Unique, case-sensitive identifier you provide to ensure the idempotency of the request. 
        [DataMember]
        public string ClientToken { get; set; }

        [DataMember]
        public bool? DisableApiTermination { get; set; }

        [DataMember]
        public bool? DryRun { get; set; }

        [DataMember]
        public bool? EbsOptimized { get; set; }

        [DataMember]
        public IamInstanceProfileSpecification IamInstanceProfile { get; set; }

        [DataMember]
        [Required]
        public string ImageId { get; set; }

        [DataMember]
        public Placement Placement { get; set; }

        // stop | terminate
        [DataMember]
        public string InstanceInitiatedShutdownBehavior { get; set; }

        // default = m1.small
        [DataMember]
        public string InstanceType { get; set; }

        [DataMember]
        public int? Ipv6AddressCount { get; set; }

        [DataMember]
        public string KernelId { get; set; }

        [DataMember]
        public string KeyName { get; set; }

        [DataMember]
        [Range(1, 100)]
        public int MaxCount { get; set; }

        [DataMember]
        [Range(1, 100)]
        public int MinCount { get; set; }

        [DataMember]
        public RunInstancesMonitoringEnabled Monitoring { get; set; }

        [DataMember]
        public string PrivateIpAddress { get; set; }

        [DataMember(Name = "SecurityGroupId")]
        public string[] SecurityGroupIds { get; set; }

        [DataMember]
        public string SubnetId { get; set; }

        [DataMember(Name = "TagSpecification")]
        public List<TagSpecification> TagSpecifications { get; set; }
 
        public Dictionary<string, string> ToParams()
        {
            return Ec2RequestHelper.ToParams("RunInstances", this);
        }
    }

    public class RunInstancesMonitoringEnabled
    {
        public RunInstancesMonitoringEnabled() { }

        public RunInstancesMonitoringEnabled(bool enabled)
        {
            Enabled = enabled;
        }

        public bool Enabled { get; set; }
    }
}
