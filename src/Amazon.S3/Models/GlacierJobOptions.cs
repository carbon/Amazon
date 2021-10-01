namespace Amazon.S3;

public sealed class GlacierJobOptions
{
    public GlacierJobOptions(GlacierJobTier tier)
    {
        Tier = tier;
    }

    public GlacierJobTier Tier { get; }
}