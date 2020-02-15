#nullable disable

namespace Amazon.Elb
{
    public sealed class RuleCondition
    {
        // host-header, path-pattern
        public string Field { get; set; }

        public string[] Values { get; set; }
    }
}
