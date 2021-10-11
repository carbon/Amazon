#nullable disable

namespace Amazon.Ssm;

public sealed class GetParameterHistoryRequest : ISsmRequest
{
    public int? MaxResults { get; set; }

    public string Name { get; set; }

    public string NextToken { get; set; }

    public bool WithDecryption { get; set; }
}