#nullable disable

using System.Collections.Generic;

namespace Amazon.Ssm
{
    public sealed class StartAutomationExecutionRequest
    {
        public string ClientToken { get; set; }

        public string DocumentName { get; set; }

        public string DocumentVersion { get; set; }

        public Dictionary<string, string> Parameters { get; set; }
    }
}