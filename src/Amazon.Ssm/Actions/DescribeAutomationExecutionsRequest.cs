namespace Amazon.Ssm
{
    public class DescribeAutomationExecutionsRequest
    {
        public AutomationExecutionFilter[] Filters { get; set; }

        public int? MaxResults { get; set; }

        public string NextToken { get; set; }
    }

    public class AutomationExecutionFilter
    {
        public string Key { get; set; }

        public string[] Values { get; set; }
    }
}