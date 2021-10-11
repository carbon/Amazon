#nullable disable

using System;
using System.Collections.Generic;

namespace Amazon.CloudWatch;

public class MetricDatum
{
    public string MetricName { get; set; }

    // Optional
    public StatisticSet StatisticValues { get; set; }

    public DateTime Timestamp { get; set; }

    public Dimension[] Dimensions { get; set; }

    public string Unit { get; set; }

    public double Value { get; set; }
}
