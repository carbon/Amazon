using Carbon.Json;
using System.Runtime.Serialization;

namespace Amazon.Ec2.Requests
{
    public class RunInstancesRequest
    {
        public string ClientToken { get; set; }

        public bool DisableApiTermination { get; set; }

        public bool EbsOptimized { get; set; }
        
        public string IamInstanceProfile { get; set; }

        public string ImageId { get; set; }

        public string InstanceInitiatedShutdownBehavior { get; set; }

        public string InstanceType { get; set; }

        public string KeyName { get; set; }

        public int MaxCount { get; set; }

        public int MinCount { get; set; }

        public string SubnetId { get; set; }

        public string UserData { get; set; }

        protected AwsRequest GetParameters()
        {
            var parameters = new AwsRequest {
                { "Action", "RunInstances" }
            };
            
            foreach (var member in JsonObject.FromObject(this))
            {
                if (member.Value is XNull) continue;

                parameters.Add(member.Key, member.Value.ToString());
            }

            return parameters;
        }
    }
}
