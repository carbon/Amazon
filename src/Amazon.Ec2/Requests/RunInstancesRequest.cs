using System.Collections.Generic;
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
        public bool DisableApiTermination { get; set; }

        [DataMember]
        public bool EbsOptimized { get; set; }

        [DataMember]
        public IamInstanceProfileSpecification IamInstanceProfile { get; set; }

        [DataMember]
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
        public int MaxCount { get; set; }

        [DataMember]
        public int MinCount { get; set; }

        [DataMember]
        public string SubnetId { get; set; }

        [DataMember]
        public string UserData { get; set; }

        [DataMember(Name ="SecurityGroupId")]
        public string[] SecurityGroupIds { get; set; }

        public AwsRequest ToParams()
        {
            var parameters = new AwsRequest {
                { "Action", "RunInstances" }
            };
            
            foreach (var member in JsonObject.FromObject(this))
            {
                if (member.Value is XNull) continue;

                if (member.Value is JsonArray array)
                {
                    for (var i = 0; i < array.Count; i++)
                    {
                        var prefix = member.Key + "." + (i + 1);

                        var element = array[i];

                        if (element is JsonObject obj)
                        {
                            AddParameters(parameters, prefix, obj);
                        }
                        else
                        {
                            parameters.Add(prefix, element.ToString());
                        }
                    }
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

        private void AddParameters(AwsRequest parameters, string prefix, JsonObject instance)
        {
            if (parameters.Parameters.Count > 100) throw new System.Exception("excedeeded max of 100 parameters");

            foreach (var m in instance)
            {
                if (m.Value is XNull) continue;

                var key = prefix + "." + m.Key;

                if (m.Value is JsonObject obj)
                {
                    AddParameters(parameters, key, obj);
                }
                else
                {
                    parameters.Add(key, m.Value.ToString());
                }
            }
        }
    }
}
