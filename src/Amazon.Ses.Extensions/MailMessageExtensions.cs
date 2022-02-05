﻿using System.Net.Mail;

using MimeKit;

namespace Amazon.Ses;

public static class MailMessageExtensions
{
    public static RawMessage ToRawMessage(this MailMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);

        var mimeMessage = MimeMessage.CreateFromMailMessage(message);

        return mimeMessage.ToRawMessage();
    }
}
