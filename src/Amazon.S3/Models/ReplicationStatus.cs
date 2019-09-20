namespace Amazon.S3
{
    public enum ReplicationStatus : byte
    {
        None      = 0,
        Pending   = 1,
        Completed = 2,
        Failed    = 3,
        Replica   = 4
    }
}