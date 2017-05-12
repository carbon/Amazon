namespace Amazon.Elb
{
    public class RuleCondition
    {
        // host-header, path-pattern
        public string Field { get; set; }

        public string[] Values { get; set; }
    }
}
