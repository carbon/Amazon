using System.Collections.Generic;
using System.Globalization;

namespace Amazon.Ses;

internal static class DestinationListHelper
{
    internal static void AddDestinationList(string type, string[]? addresses, List<KeyValuePair<string, string>> dic)
    {
        // http://www.ietf.org/rfc/rfc0822.txt

        if (addresses is null || addresses.Length is 0) return;

        int i = 1;

        foreach (var address in addresses)
        {
            // &Destination.ToAddresses.member.1=allan%40example.com

            dic.Add(new (string.Create(CultureInfo.InvariantCulture, $"Destination.{type}Addresses.member.{i}"), address));

            i++;
        }
    }

    internal static IEnumerable<KeyValuePair<string, string>> GetReplyToAddresses(string[]? addresses)
    {
        if (addresses is null) yield break;

        for (int i = 0; i < addresses.Length; i++)
        {
            yield return new (string.Create(CultureInfo.InvariantCulture, $"ReplyToAddresses.member.{i + 1}"), addresses[i]);
        }
    }
}

internal sealed class RecipientType
{
    public const string To  = "To";
    public const string Cc  = "Cc";
    public const string Bcc = "Bcc";
}
