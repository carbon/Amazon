#nullable disable

using System.Collections.Generic;

namespace Amazon.Ssm;

public sealed class FailureDetails
{
    public Dictionary<string, object> Details { get; init; }

    public string FailureStage { get; init; }

    public string FailureType { get; init; }
}