#nullable disable

namespace Amazon.Ssm;

public sealed class DescribeActivationsFilter
{
    public string FilterKey { get; set; }

    public string[] FilterValues { get; set; }
}
