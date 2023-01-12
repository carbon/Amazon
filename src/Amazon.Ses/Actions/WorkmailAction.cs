namespace Amazon.Ses;

public sealed class WorkmailAction
{
    public WorkmailAction(string organizationArn, string? topicArn = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(organizationArn);

        OrganizationArn = organizationArn;
        TopicArn = topicArn;
    }

    public string OrganizationArn { get; }

    public string? TopicArn { get; }
}