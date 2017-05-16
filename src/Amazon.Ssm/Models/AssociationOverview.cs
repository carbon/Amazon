using Carbon.Json;

namespace Amazon.Ssm
{
    public class AssociationOverview
    {
        public JsonObject AssociationStatusAggregatedCount { get; set; }

        public string DetailedStatus { get; set; }

        public string Status { get; set; }
    }
}
