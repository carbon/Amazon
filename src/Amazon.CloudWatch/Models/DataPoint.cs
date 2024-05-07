#nullable disable

namespace Amazon.CloudWatch;

public sealed class DataPoint
{
    public double? Average { get; set; }

    public double? Maximum { get; set; }

    public double? Minimum { get; set; }

    public double? SampleCount { get; set; }

    public double? Sum { get; set; }

    public DateTime Timestamp { get; set; }

    public string Unit { get; set; }
}