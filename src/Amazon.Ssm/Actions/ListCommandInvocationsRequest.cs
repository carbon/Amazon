#nullable disable

namespace Amazon.Ssm
{
    public sealed class ListCommandInvocationsRequest : ISsmRequest
    {
        public string CommandId { get; set; }

        public bool? Details { get; set; }

        public CommandFilter[] Filters { get; set; }

        public string InstanceId { get; set; }

        public int? MaxResults { get; set; }

        public string NextToken { get; set; }
    }
}