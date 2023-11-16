namespace Amazon.S3;

public sealed class GlacierJobOptions(GlacierJobTier tier)
{
    public GlacierJobTier Tier { get; } = tier;
}