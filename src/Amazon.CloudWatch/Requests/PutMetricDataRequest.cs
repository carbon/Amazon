using System.Globalization;

using Amazon.CloudWatch.Serialization;

namespace Amazon.CloudWatch;

public sealed class PutMetricDataRequest : List<MetricDatum>
{
    public required string Namespace { get; init; }

    internal List<KeyValuePair<string, string>> ToParameters()
    {
        var parameters = new List<KeyValuePair<string,string>> {
            new("Action", "PutMetricData")
        };

        if (Namespace != null)
        {
            parameters.Add(new("Namespace", Namespace));
        }

        for (int i = 0; i < Count; i++)
        {
            var datum = this[i];
            int number = i + 1;

            var prefix = string.Create(CultureInfo.InvariantCulture, $"MetricData.member.{number}");

            parameters.Add(new($"{prefix}.MetricName", datum.MetricName));

            if (datum.Unit != null)
            {
                parameters.Add(new($"{prefix}.Unit", datum.Unit));
            }

            if (datum.Timestamp != default)
            {
                parameters.Add(new($"{prefix}.Timestamp", CloudWatchTimestamp.Format(datum.Timestamp)));
            }

            if (datum.StatisticValues is { } statistics)
            {
                parameters.Add(new($"{prefix}.StatisticValues.Maximum",     statistics.Maximum.ToString(CultureInfo.InvariantCulture)));
                parameters.Add(new($"{prefix}.StatisticValues.Minimum",     statistics.Minimum.ToString(CultureInfo.InvariantCulture)));
                parameters.Add(new($"{prefix}.StatisticValues.SampleCount", statistics.SampleCount.ToString(CultureInfo.InvariantCulture)));
                parameters.Add(new($"{prefix}.StatisticValues.Sum",         statistics.Sum.ToString(CultureInfo.InvariantCulture)));
            }
            else
            {
                parameters.Add(new($"{prefix}.Value", datum.Value.ToString(CultureInfo.InvariantCulture)));
            }

            if (datum.Dimensions != null)
            {
                for (int i2 = 0; i2 < datum.Dimensions.Length; i2++)
                {
                    var dimension = datum.Dimensions[i2];

                    var prefix2 = string.Create(CultureInfo.InvariantCulture, $"{prefix}.Dimensions.member.{i2 + 1}");

                    parameters.Add(new($"{prefix2}.Name", dimension.Name));
                    parameters.Add(new($"{prefix2}.Value", dimension.Value));
                }
            }
        }

        return parameters;
    }
}
