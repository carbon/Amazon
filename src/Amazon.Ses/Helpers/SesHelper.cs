using System.Net.Mail;

namespace Amazon.Ses;

public static class SesHelper
{
    public static string EncodeMailAddress(MailAddress email)
    {
        string result = QuotedPrintable.Encode(email.ToString());

        // TODO: Verify Length <= 320

        return result;
    }

    internal static string[] EncodeMailAddressCollection(MailAddressCollection collection)
    {
        if (collection.Count == 0) return Array.Empty<string>();

        var addresses = new string[collection.Count];

        for (int i = 0; i < collection.Count; i++)
        {
            addresses[i] = EncodeMailAddress(collection[i]);
        }

        return addresses;
    }
}