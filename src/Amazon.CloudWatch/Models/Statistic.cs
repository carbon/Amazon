namespace Amazon.CloudWatch
{
    public class Statistic
    {
        public static readonly Statistic SampleCount = new Statistic("SampleCount");
        public static readonly Statistic Average     = new Statistic("Average");
        public static readonly Statistic Sum         = new Statistic("Sum");
        public static readonly Statistic Maximum     = new Statistic("Maximum");
        public static readonly Statistic Minimum     = new Statistic("Minimum");

        private Statistic(string name)
        {
            Name = name;    
        }

        public string Name { get; }

        public override string ToString() => Name;
    }
}
