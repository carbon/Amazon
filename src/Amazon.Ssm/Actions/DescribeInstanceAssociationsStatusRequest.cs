#nullable disable

namespace Amazon.Ssm;

public sealed class DescribeInstanceAssociationsStatusRequest : ISsmRequest
{
    public string InstanceId { get; set; }

    public int? MaxResults { get; set; }

    public string NextToken { get; set; }
}