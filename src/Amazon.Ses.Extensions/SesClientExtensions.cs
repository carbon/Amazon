using MimeKit;

namespace Amazon.Ses;

public static class SesClientExtensions
{
    public static async Task SendEmailAsync(this SesClient client, MimeMessage message)
    {
        var raw = new SendRawEmailRequest(message.ToRawMessage());

        await client.SendRawEmailAsync(raw);
    }
}