using System;
using System.Collections.Generic;

namespace Amazon.Ses;

public sealed class SendRawEmailRequest
{
    public string? ConfigurationSetName { get; set; }

    public string? Source { get; set; }

    public string? FromArn { get; set; }

    public string? ReturnPathArn { get; set; }

    public string[]? To { get; set; }

    public string[]? BCC { get; set; }

    public string[]? CC { get; set; }

#nullable disable

    public RawMessage RawMessage { get; set; }

    public List<KeyValuePair<string, string>> ToParams()
    {
        var dic = new List<KeyValuePair<string, string>>(6);

        if (ConfigurationSetName != null)
        {
            dic.Add(new ("ConfigurationSetName", Source));
        }

        if (Source != null)
        {
            dic.Add(new("Source", Source));
        }

        if (FromArn != null)
        {
            dic.Add(new("FromArn", FromArn));
        }

        if (ReturnPathArn != null)
        {
            dic.Add(new("ReturnPathArn", FromArn));
        }

        dic.Add(new("RawMessage.Data", Convert.ToBase64String(RawMessage.Data)));

        DestinationListHelper.AddDestinationList(RecipientType.To, To, dic);
        DestinationListHelper.AddDestinationList(RecipientType.Cc, CC, dic);
        DestinationListHelper.AddDestinationList(RecipientType.Bcc, BCC, dic);        

        return dic;
    }
}