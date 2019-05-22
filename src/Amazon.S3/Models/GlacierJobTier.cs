namespace Amazon.S3
{
    public enum GlacierJobTier : byte
    {
        Standard  = 0, // $0.01 per GB   + $0.05 per 1,000 requests  [3-5 hours]
        Expedited = 1, // $0.03 per GP   + $10.00 per 1000 requests  [1-5 minutes]
        Bulk      = 2  // $0.0025 per GB + $0.025 per 1,000 requests [5-12 hours]
    }
}