namespace Amazon.Ssm
{
    public enum CommandStatus
    {
        Pending,
        InProgress,
        Success,
        Cancelled,
        Failed,
        TimedOut,
        Cancelling
    }
}