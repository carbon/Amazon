#nullable disable

using System.Collections.Generic;

namespace Amazon.Ssm
{
    public sealed class UpdateAssociationRequest : ISsmRequest
    {
        public string AssociationId { get; set; }

        public string DocumentVersion { get; set; }

        public OutputLocation OutputLocation { get; set; }

        public Dictionary<string, object> Parameters { get; set; }

        public string ScheduleExpression { get; set; }
    }

    public class OutputLocation
    {
        public S3Location S3Location { get; set; }
    }
}