using Amazon.CloudWatch.Serialization;

namespace Amazon.CloudWatch;

public sealed class DescribeAlarmsRequest
{
    public string? ActionPrefix { get; init; }

    /// <summary>
    /// Cannot be combined with <see cref="AlarmNames"/>.
    /// </summary>
    public string? AlarmNamePrefix { get; init; }

    public IReadOnlyList<string>? AlarmNames { get; init; }

    /// <summary>
    /// The maximum number of alarms to return, from 1 to 100.
    /// </summary>
    public int? MaxRecords { get; init; }

    public string? NextToken { get; init; }

    public string? StateValue { get; init; }

    internal List<KeyValuePair<string, string>> ToParameters()
    {
        var parameters = new List<KeyValuePair<string, string>> {
            new("Action", "DescribeAlarms")
        };

        parameters.AddOptional("ActionPrefix", ActionPrefix);
        parameters.AddOptional("AlarmNamePrefix", AlarmNamePrefix);
        parameters.AddMembers("AlarmNames", AlarmNames);
        parameters.AddOptional("MaxRecords", MaxRecords);
        parameters.AddOptional("NextToken", NextToken);
        parameters.AddOptional("StateValue", StateValue);

        return parameters;
    }
}
