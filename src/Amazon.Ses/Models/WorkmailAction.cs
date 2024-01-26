namespace Amazon.Ses;

public sealed class WorkmailAction(
    string organizationArn,
    string? topicArn = null)
{
    public string OrganizationArn { get; } = organizationArn ?? throw new ArgumentNullException(nameof(organizationArn));

    public string? TopicArn { get; } = topicArn;
}