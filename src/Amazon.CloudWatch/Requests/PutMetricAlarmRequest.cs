namespace Amazon.CloudWatch;

public sealed class PutMetricAlarmRequest
{
    public string? Namespace { get; init; }

    public bool? ActionsEnabled { get; init; }

    internal List<KeyValuePair<string, string>> ToParameters()
    {
        var parameters = new List<KeyValuePair<string, string>> {
            new ("Action", "PutMetricAlarm")
        };

        // TODO

        return parameters;
    }
}
