namespace Amazon.CloudWatch;

public sealed class StatisticSet
{
    public double Maximum { get; init; }

    public double Minimum { get; init; }

    public double SampleCount { get; init; }

    public double Sum { get; init; }
}