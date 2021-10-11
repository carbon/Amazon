#nullable disable

namespace Amazon.Ssm;

public sealed class GetParametersRequest : ISsmRequest
{
    public string[] Names { get; init; }

    public bool? WithDecryption { get; init; }
}
