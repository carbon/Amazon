namespace Amazon.Ses;

public class GetAccountResult
{
    public required SendQuota SendQuota { get; init; }
}

public class SendQuota
{
    public double Max24HourSend { get; init; }

    public double MaxSendRate { get; init; }

    public double SentLast24Hours { get; init; }
}