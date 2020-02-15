using System.Text.Json.Serialization;
using Amazon.Ssm.Converters;

namespace Amazon.Ssm
{
    public sealed class InstanceInformation
    {
        public string ActivationId { get; set; }

        public string AgentVersion { get; set; }

        public string AssociationStatus { get; set; }

        public string ComputerName { get; set; }

        public string InstanceId { get; set; }

        public string IamRole { get; set; }

        public bool IsLatestVersion { get; set; }

        [JsonConverter(typeof(NullableTimestampConverter))]
        public Timestamp? LastAssociationExecutionDate { get; set; }

        [JsonConverter(typeof(NullableTimestampConverter))]
        public Timestamp? LastPingDateTime { get; set; }

        [JsonConverter(typeof(NullableTimestampConverter))]
        public Timestamp? LastSuccessfulAssociationExecutionDate { get; set; }

        public string Name { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PingStatus PingStatus { get; set; }

        public string PlatformName { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PlatformType PlatformType { get; set; }

        public string PlatformVersion { get; set; }

        [JsonConverter(typeof(NullableTimestampConverter))]
        public Timestamp? RegistrationDate { get; set; }

        // ManagedInstance | Document | EC2Instance
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ResourceType ResourceType { get; set; }
    }
}
