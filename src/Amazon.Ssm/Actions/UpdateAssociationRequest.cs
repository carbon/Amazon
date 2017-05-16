using Carbon.Json;

namespace Amazon.Ssm
{
    public class UpdateAssociationRequest : ISsmRequest
    {
        public string AssociationId { get; set; }

        public string DocumentVersion { get; set; }

        public OutputLocation OutputLocation { get; set; }

        public JsonObject Parameters { get; set; }

        public string ScheduleExpression { get; set; }
    }

    public class OutputLocation
    {
        public S3Location S3Location { get; set; }
    }
}