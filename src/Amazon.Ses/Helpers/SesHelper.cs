using System.Net.Mail;

namespace Amazon.Ses;

public static class SesHelper
{
    // TODO: Avoid these allocations

    public static string EncodeMailAddress(MailAddress email)
    {
        return QuotedPrintable.Encode(email.ToString());
    }
}