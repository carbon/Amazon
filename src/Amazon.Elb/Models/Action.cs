#nullable disable

namespace Amazon.Elb
{
    public sealed class Action
    {
        public Action() { }

        public Action(string targetGroupArn, string type = "forward")
        {
            TargetGroupArn = targetGroupArn;
            Type = type;
        }

        public string TargetGroupArn { get; init; }

        // forward
        public string Type { get; init; }
    }
}