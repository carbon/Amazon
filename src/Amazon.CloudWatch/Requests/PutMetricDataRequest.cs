using System.Globalization;

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
            parameters.Add(new($"{prefix}.Unit",       datum.Unit));
            parameters.Add(new($"{prefix}.Value",      datum.Value.ToString()));

            if (datum.Dimensions != null)
            {
                for (int i2 = 0; i < datum.Dimensions.Length; i++)
                {
                    var dimension = datum.Dimensions[i2];

                    var prefix2 = string.Create(CultureInfo.InvariantCulture, $"{prefix}Dimensions.member.{i2 + 1}");

                    parameters.Add(new($"{prefix2}.Name", dimension.Name));
                    parameters.Add(new($"{prefix2}.Value", dimension.Name));
                }
            }
        }

        return parameters;
    }
}
