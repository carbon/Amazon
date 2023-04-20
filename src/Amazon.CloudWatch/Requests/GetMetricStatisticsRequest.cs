using System.Globalization;

namespace Amazon.CloudWatch;

public sealed class GetMetricStatisticsRequest
{
    public GetMetricStatisticsRequest(string nameSpace, string metricName)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameSpace);
        ArgumentException.ThrowIfNullOrEmpty(metricName);

        Namespace = nameSpace;
        MetricName = metricName;
    }

    // Required
    public string MetricName { get; }

    // Required
    public string Namespace { get; }

    public Dimension[]? Dimensions { get; set; }

    public Statistic[]? Statistics { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string? Unit { get; set; }

    /// <summary>
    /// The granularity, in seconds, of the returned data points. 
    /// A period can be as short as one minute (60 seconds) and must be a multiple of 60.
    /// The default value is 60.
    /// </summary>
    public TimeSpan Period { get; set; } = TimeSpan.FromSeconds(60);

    internal List<KeyValuePair<string, string>> ToParameters()
    {
        var parameters = new List<KeyValuePair<string, string>> {
            new("Action", "GetMetricStatistics"),

            // Required parameters
            new("Namespace",  Namespace),
            new("MetricName", MetricName),
            new("StartTime",  StartTime.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture)),
            new("EndTime",    EndTime.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture)),
            new("Period",     ((int)Period.TotalSeconds).ToString(CultureInfo.InvariantCulture))
        };

        if (Unit != null)
        {
            parameters.Add(new("Unit", Unit));
        }

        if (Dimensions != null)
        {
            for (int i = 0; i < Dimensions.Length; i++)
            {
                var dimension = Dimensions[i];
                int number = (i + 1);

                string prefix = string.Create(CultureInfo.InvariantCulture, $"Dimensions.member.{number}");

                parameters.Add(new($"{prefix}.Name", dimension.Name));
                parameters.Add(new($"{prefix}.Value", dimension.Value));
            }
        }

        if (Statistics != null)
        {
            for (int i = 0; i < Statistics.Length; i++)
            {
                var stat = Statistics[i];
                int number = i + 1;

                parameters.Add(new (string.Create(CultureInfo.InvariantCulture, $"Statistics.member.{number}"), stat.Name));
            }
        }

        return parameters;
    }
}
