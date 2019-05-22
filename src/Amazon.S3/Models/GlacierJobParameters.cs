namespace Amazon.S3
{
    public sealed class GlacierJobParameters
    {
        public GlacierJobParameters(GlacierJobTier tier)
        {
            this.Tier = tier;
        }

        public GlacierJobTier Tier { get; }
    }
}