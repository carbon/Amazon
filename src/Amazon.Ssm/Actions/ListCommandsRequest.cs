namespace Amazon.Ssm
{
    public class ListCommandsRequest : ISsmRequest
    {
        public string CommandId { get; set; }

        public CommandFilter[] Filters { get; set; }

        public string InstanceId { get; set; }

        public int? MaxResults { get; set; }

        public string NextToken { get; set; }
    }
}