namespace Amazon.CloudWatch;

public sealed class Statistic
{
    public static readonly Statistic SampleCount = new ("SampleCount");
    public static readonly Statistic Average     = new ("Average");
    public static readonly Statistic Sum         = new ("Sum");
    public static readonly Statistic Maximum     = new ("Maximum");
    public static readonly Statistic Minimum     = new ("Minimum");

    private Statistic(string name)
    {
        Name = name;    
    }

    public string Name { get; }

    public override string ToString() => Name;
}