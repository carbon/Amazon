#nullable disable

namespace Amazon.CloudWatch;

public sealed class PutMetricAlarmRequest
{
    public string Namespace { get; init; }

    public bool ActionsEnabled { get; init; }

    public AwsRequest ToParams()
    {
        var parameters = new AwsRequest {
            { "Action", "PutMetricAlarm" }
        };

        // TODO

        return parameters;
    }
}
