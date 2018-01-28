using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Amazon.Ec2
{
    public class RunInstancesRequest : IEc2Request
    {
        public RunInstancesRequest() { }

        public RunInstancesRequest(
            int minCount,
            int maxCount, 
            string keyName = null,
            string imageId = null,
            string subnetId = null, 
            string userData = null,
            string clientToken = null)
        {
            if (minCount <= 0)
                throw new ArgumentException("Must be > 0", nameof(minCount));

            if (maxCount <= 0)
                throw new ArgumentException("Must be > 0", nameof(maxCount));

            MinCount    = minCount;
            MaxCount    = maxCount;
            KeyName     = keyName;
            ImageId     = imageId;
            SubnetId    = subnetId;
            UserData    = userData;
            ClientToken = clientToken;
        }

        [DataMember(Name = "BlockDeviceMapping", Order = 1)]
        public BlockDeviceMapping[] BlockDeviceMappings { get; set; }

        // Unique, case-sensitive identifier you provide to ensure the idempotency of the request. 
        [DataMember(Order = 2)]
        public string ClientToken { get; set; }

        [DataMember(Order = 3)]
        public bool? DisableApiTermination { get; set; }

        [DataMember(Order = 4)]
        public bool? DryRun { get; set; }

        [DataMember(Order = 5)]
        public bool? EbsOptimized { get; set; }

        [DataMember(Order = 6)]
        public IamInstanceProfileSpecification IamInstanceProfile { get; set; }

        // [Required]
        [DataMember(Order = 7)]
        public string ImageId { get; set; }
        
        // stop | terminate
        [DataMember(Order = 8)]
        public string InstanceInitiatedShutdownBehavior { get; set; }

        // default = m1.small
        [DataMember(Order = 9)]
        public string InstanceType { get; set; }

        [DataMember(Order = 10)]
        public int? Ipv6AddressCount { get; set; }

        [DataMember(Order = 11)]
        public string KernelId { get; set; }

        [DataMember(Order = 12)]
        public string KeyName { get; set; }

        [DataMember(Order = 13)]
        // [Range(1, 100)]
        public int MaxCount { get; set; }

        [DataMember(Order = 14)]
        // [Range(1, 100)]
        public int MinCount { get; set; }

        [DataMember(Order = 15)]
        public RunInstancesMonitoringEnabled Monitoring { get; set; }

        [DataMember(Order = 16)]
        public Placement Placement { get; set; }

        [DataMember(Order = 17)]
        public string PrivateIpAddress { get; set; }

        [DataMember(Name = "SecurityGroupId", Order = 18)]
        public string[] SecurityGroupIds { get; set; }

        [DataMember(Order = 19)]
        public string SubnetId { get; set; }

        [DataMember(Name = "TagSpecification", Order = 20)]
        public TagSpecification[] TagSpecifications { get; set; }
 
        [DataMember(Order = 21)]
        public string UserData { get; set; }

        public Dictionary<string, string> ToParams()
        {
            return Ec2RequestHelper.ToParams("RunInstances", this);
        }
    }
}
