using MimeKit;

namespace Amazon.Ses;

public static class SesClientExtensions
{
    public static async Task<SendRawEmailResult> SendEmailAsync(this SesClient client, MimeMessage message)
    {
        var raw = new SendRawEmailRequest(message.ToRawMessage());

        return await client.SendRawEmailAsync(raw);
    }
}