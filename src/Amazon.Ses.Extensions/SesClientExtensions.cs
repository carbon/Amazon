using MimeKit;

namespace Amazon.Ses;

public static class SesClientExtensions
{
    public static async Task<SendEmailResult> SendEmailAsync(this SesClient client, MimeMessage message)
    {
        var request = new SendEmailRequest {

            Content = message.ToRawMessage()
        };
        
        return await client.SendEmailAsync(request);
    }
}