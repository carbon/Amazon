namespace Amazon.Ses;

public sealed class ReceiptAction
{
    /// <summary>
    /// Adds a header to the received email.
    /// </summary>
    public AddHeaderAction? AddHeaderAction { get; init; }

    /// <summary>
    /// Rejects the received email by returning a bounce response to the sender and, optionally, publishes a notification to Amazon Simple Notification Service (Amazon SNS).
    /// </summary>
    public BounceAction? BounceAction { get; init; }

    /// <summary>
    /// Calls an AWS Lambda function, and optionally, publishes a notification to Amazon SNS.
    /// </summary>
    public LambdaAction? LambdaAction { get; init; }

    /// <summary>
    /// Saves the received message to an Amazon Simple Storage Service (Amazon S3) bucket and, optionally, publishes a notification to Amazon SNS.
    /// </summary>
    public S3Action? S3Action { get; init; }

    /// <summary>
    /// Publishes the email content within a notification to Amazon SNS.
    /// </summary>
    public SnsAction? SNSAction { get; init; }

    /// <summary>
    /// Terminates the evaluation of the receipt rule set and optionally publishes a notification to Amazon SNS.
    /// </summary>
    public StopAction? StopAction { get; init; }

    /// <summary>
    /// Calls Amazon WorkMail and, optionally, publishes a notification to Amazon Amazon SNS.
    /// </summary>
    public WorkmailAction? WorkmailAction { get; init; }
}
