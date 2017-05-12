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

        [DataMember]
        public string InstanceType { get; set; }

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

        [DataMember(Name ="SecurityGroupId")]
        public string[] SecurityGroupIds { get; set; }

        [DataMember]
        public string SubnetId { get; set; }
        
        [DataMember(Name = "TagSpecification")]
        public List<TagSpecification> TagSpecifications { get; set; }

        [DataMember]
        public string UserData { get; set; }
        
        public Dictionary<string, string> ToParams()
        {
            var parameters = new Dictionary<string, string> {
                { "Action", "RunInstances" }
            };
            
            foreach (var member in JsonObject.FromObject(this))
            {
                if (member.Value is XNull) continue;

                if (member.Value is JsonArray arr)
                {
                    AddParameters(parameters, member.Key, arr);
                }
                else if (member.Value is JsonObject obj)
                {
                    AddParameters(parameters, member.Key, obj);
                }
                else
                {
                    parameters.Add(member.Key, member.Value.ToString());
                }
            }

            return parameters;
        }

        private void AddParameters(Dictionary<string, string> parameters, string prefix, JsonArray array)
        {
            for (var i = 0; i < array.Count; i++)
            {
                var key = prefix + "." + (i + 1);

                var element = array[i];

                if (element is JsonObject obj)
                {
                    AddParameters(parameters, key, obj);
                }
                else
                {
                    parameters.Add(key, element.ToString());
                }
            }
        }

        private void AddParameters(Dictionary<string, string> parameters, string prefix, JsonObject instance)
        {
            if (parameters.Count > 100) throw new System.Exception("excedeeded max of 100 parameters");

            foreach (var m in instance)
            {
                if (m.Value is XNull) continue;

                var key = prefix + "." + m.Key;

                if (m.Value is JsonObject obj)
                {
                    AddParameters(parameters, key, obj);
                }
                else if (m.Value is JsonArray arr)
                {
                    AddParameters(parameters, key, arr);
                }
                else
                {
                    parameters.Add(key, m.Value.ToString());
                }
            }
        }
    }


    public class TagSpecification
    {
        // instance and volume.
        public string ResourceType { get; set; }

        [DataMember(Name = "Tag")]
        public Tag[] Tags { get; set; }
    }

    public class Tag
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }

    public class RunInstancesMonitoringEnabled
    {
        public string Enabled { get; set; }
    }
}
