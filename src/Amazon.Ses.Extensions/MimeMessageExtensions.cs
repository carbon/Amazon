
using MimeKit;

namespace Amazon.Ses;

public static class MimeMessageExtensions
{
    public static RawMessage ToRawMessage(this MimeMessage message)
    {
        using var ms = new MemoryStream();

        message.WriteTo(ms);

        return new RawMessage(ms.ToArray());
    }
}