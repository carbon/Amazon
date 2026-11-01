using Amazon.CloudWatch.Serialization;

namespace Amazon.CloudWatch;

public sealed class SetAlarmStateRequest
{
    public required string AlarmName { get; init; }

    public required string StateValue { get; init; }

    public required string StateReason { get; init; }

    /// <summary>
    /// The reason in JSON format. Auto Scaling alarm actions use it to choose the correct action.
    /// </summary>
    public string? StateReasonData { get; init; }

    internal List<KeyValuePair<string, string>> ToParameters()
    {
        var parameters = new List<KeyValuePair<string, string>> {
            new("Action", "SetAlarmState"),
            new("AlarmName", AlarmName),
            new("StateValue", StateValue),
            new("StateReason", StateReason)
        };

        parameters.AddOptional("StateReasonData", StateReasonData);

        return parameters;
    }
}
