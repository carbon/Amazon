using Carbon.Json;

namespace Amazon.Ssm
{
    public class StartAutomationExecutionRequest
    {
        public string ClientToken { get; set; }

        public string DocumentName { get; set; }

        public string DocumentVersion { get; set; }

        public JsonObject Parameters { get; set; }
    }
}