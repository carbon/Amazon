#nullable disable

using System.Collections.Generic;

namespace Amazon.Ssm;

public sealed class CreateAssociationRequest : ISsmRequest
{
    public string DocumentVersion { get; set; }

    public string InstanceId { get; set; }

    public OutputLocation OutputLocation { get; set; }

    public Dictionary<string, string> Parameters { get; set; }

    public string ScheduleExpression { get; set; }

    public Target[] Targets { get; set; }
}
