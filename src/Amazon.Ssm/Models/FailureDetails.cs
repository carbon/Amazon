#nullable disable

using System.Collections.Generic;

namespace Amazon.Ssm
{
    public sealed class FailureDetails
    {
        public Dictionary<string, object> Details { get; set; }

        public string FailureStage { get; set; }

        public string FailureType { get; set; }
    }
}
