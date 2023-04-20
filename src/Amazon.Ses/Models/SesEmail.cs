#nullable disable

using System.IO;
using System.Linq;
using System.Net.Mail;

namespace Amazon.Ses;

public sealed class SesEmail
{
    public string Source { get; set; }

    public string[] ReplyTo { get; set; }

    public string[] To { get; set; }

#nullable enable
    public string[]? Bcc { get; set; }

    public string[]? Cc { get; set; }

#nullable disable

    public SesContent Subject { get; set; }

#nullable enable

    public SesContent? Html { get; set; }

    public SesContent? Text { get; set; }

    public List<KeyValuePair<string, string>> ToParameters()
    {
        var result = new List<KeyValuePair<string, string>>(16) {
            new("Source", Source)
        };

        SetContent("Message.Subject", Subject, result);

        if (Html is not null)
        {
            SetContent("Message.Body.Html", Html, result);
        }

        if (Text is not null)
        {
            SetContent("Message.Body.Text", Text, result);
        }

        foreach (var p in DestinationListHelper.GetReplyToAddresses(ReplyTo))
        {
            result.Add(p);
        }

        DestinationListHelper.AddDestinationList(RecipientType.To,  To,  result);
        DestinationListHelper.AddDestinationList(RecipientType.Cc,  Cc,  result);
        DestinationListHelper.AddDestinationList(RecipientType.Bcc, Bcc, result);
        
        return result;
    }

    public static SesEmail FromMailMessage(MailMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);

        var doc = new SesEmail {
            Source = SesHelper.EncodeMailAddress(message.From!),
            To = message.To.Select(r => SesHelper.EncodeMailAddress(r)).ToArray(),
            Subject = new SesContent(message.Subject, CharsetType.UTF8)
        };

        // "Carbonmade" <hello@carbonmade.com>

        if (message.ReplyToList.Count > 0)
        {
            doc.ReplyTo = SesHelper.EncodeMailAddressCollection(message.ReplyToList);
        }
#pragma warning disable CS0618 // Type or member is obsolete
        else if (message.ReplyTo != null)
        {
            doc.ReplyTo = new[] { SesHelper.EncodeMailAddress(message.ReplyTo) };
        }
#pragma warning restore CS0618


        if (message.IsBodyHtml)
        {
            doc.Html = new SesContent(message.Body, CharsetType.UTF8);
        }
        else
        {
            doc.Text = new SesContent(message.Body, CharsetType.UTF8);
        }

        if (message.CC.Count > 0)
        {
            doc.Cc = SesHelper.EncodeMailAddressCollection(message.CC);
        }

        if (message.Bcc.Count > 0)
        {
            doc.Cc = SesHelper.EncodeMailAddressCollection(message.Bcc);
        }

        // Alternate view support
        if (message.AlternateViews != null)
        {
            foreach (var view in message.AlternateViews)
            {
                using var streamReader = new StreamReader(view.ContentStream);

                string text = streamReader.ReadToEnd();

                switch (view.ContentType.MediaType)
                {
                    case "text/plain" : doc.Text = new SesContent(text, CharsetType.UTF8); break;
                    case "text/html"  : doc.Html = new SesContent(text, CharsetType.UTF8); break;
                }
            }
        }

        return doc;
    }   

    private static void SetContent(string prefix, SesContent content, List<KeyValuePair<string, string>> dic)
    {
        if (content is null) return;

        dic.Add(new($"{prefix}.Data", content.Data));

        if (content.Charset is not null)
        {
            dic.Add(new($"{prefix}.Charset", content.Charset));
        }
    }

    /*
    AWSAccessKeyId=AKIAIOSFODNN7EXAMPLE
    &Action=SendEmail
    &Destination.ToAddresses.member.1=allan%40example.com
    &Message.Body.Text.Data=body
    &Message.Subject.Data=Example&Source=user%40example.com
    &Timestamp=2011-08-18T22%3A25%3A27.000Z
    */
}
