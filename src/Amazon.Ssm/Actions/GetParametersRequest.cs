#nullable disable

namespace Amazon.Ssm
{
    public sealed class GetParametersRequest : ISsmRequest
    {
        public string[] Names { get; set; }

        public bool? WithDecryption { get; set; }
    }
}