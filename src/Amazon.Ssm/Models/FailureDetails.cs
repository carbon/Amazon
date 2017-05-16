using Carbon.Json;

namespace Amazon.Ssm
{
    public class FailureDetails
    {
        public JsonObject Details { get; set; }

        public string FailureStage { get; set; }

        public string FailureType { get; set; }
    }
}
