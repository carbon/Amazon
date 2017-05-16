namespace Amazon.Elb
{
    public class Action
    {
        public Action() { }

        public Action(string targetGroupArn, string type = "forward")
        {
            TargetGroupArn = targetGroupArn;
            Type = type;
        }

        public string TargetGroupArn { get; set; }

        // forward
        public string Type { get; set; }
    }
}