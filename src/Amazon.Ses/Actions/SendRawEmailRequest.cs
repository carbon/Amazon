using System.Diagnostics.CodeAnalysis;

namespace Amazon.Ses;

public sealed class SendRawEmailRequest
{
    public SendRawEmailRequest() { }

    [SetsRequiredMembers]
    public SendRawEmailRequest(RawMessage rawMessage)
    {
        RawMessage = rawMessage;
    }

    public string? ConfigurationSetName { get; set; }

    public string? Source { get; set; }

    public string? FromArn { get; set; }

    public string? ReturnPathArn { get; set; }

    public string[]? To { get; set; }

    public string[]? BCC { get; set; }

    public string[]? CC { get; set; }

    public required RawMessage RawMessage { get; set; }

    public List<KeyValuePair<string, string>> ToParams()
    {
        var parameters = new List<KeyValuePair<string, string>>(8);

        if (ConfigurationSetName is not null)
        {
            parameters.Add(new("ConfigurationSetName", ConfigurationSetName));
        }

        if (Source is not null)
        {
            parameters.Add(new("Source", Source));
        }

        if (FromArn is not null)
        {
            parameters.Add(new("FromArn", FromArn));
        }

        if (ReturnPathArn is not null)
        {
            parameters.Add(new("ReturnPathArn", ReturnPathArn));
        }

        parameters.Add(new("RawMessage.Data", Convert.ToBase64String(RawMessage.Data)));

        DestinationListHelper.AddDestinationList(RecipientType.To, To, parameters);
        DestinationListHelper.AddDestinationList(RecipientType.Cc, CC, parameters);
        DestinationListHelper.AddDestinationList(RecipientType.Bcc, BCC, parameters);

        return parameters;
    }
}