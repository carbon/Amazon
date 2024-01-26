using System.Globalization;
using System.Net.Mail;
using System.Text;

namespace Amazon.Ses;

public static class SesHelper
{
    private static readonly IdnMapping s_idn = new();

    // Amazon SES does not support the SMTPUTF8 extension, as described in RFC6531.
    // For this reason, the local part of a destination email address (the part of the email address that precedes the @ sign) may only contain 7-bit ASCII characters.

    public static bool IsSupported(MailAddress email)
    {
        return Ascii.IsValid(email.User);
    }

    public static string DecodeMailAddress(string address)
    {
        try
        {
            if (address.StartsWith("=?UTF", StringComparison.OrdinalIgnoreCase))
            {
                return new MailAddress(QuotedPrintable.Decode(address)).Address.ToLowerInvariant();
            }

            return new MailAddress(address).Address.ToLowerInvariant();
        }
        catch
        {
            throw new Exception($"error parsing email - '{address}'");
        }
    }

    public static string EncodeMailAddress(MailAddress email)
    {
        var result = new ValueStringBuilder(128);

        // "Sender Name" <sender@example.com>

        bool hasDisplayName = !string.IsNullOrWhiteSpace(email.DisplayName);

        if (hasDisplayName && email.DisplayName.Length < 128)
        {
            result.Append('"');
            result.Append(QuotedPrintable.Encode(email.DisplayName!));
            result.Append('"');
            result.Append(' ');
        }

        if (hasDisplayName)
        {
            result.Append('<');
        }

        result.Append(email.User);
        result.Append('@');

        if (Ascii.IsValid(email.Host))
        {
            result.Append(email.Host);
        }
        else
        {
            result.Append(s_idn.GetAscii(email.Host));
        }

        if (hasDisplayName)
        {
            result.Append('>');
        }

        // TODO: Verify Length <= 320

        return result.ToString();
    }

    internal static string[] EncodeMailAddressCollection(MailAddressCollection collection)
    {
        if (collection.Count is 0) return [];

        var addresses = new string[collection.Count];

        for (int i = 0; i < collection.Count; i++)
        {
            addresses[i] = EncodeMailAddress(collection[i]);
        }

        return addresses;
    }
}