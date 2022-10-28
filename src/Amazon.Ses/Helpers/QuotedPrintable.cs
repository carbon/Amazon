using System.Buffers;
using System.Net.Mail;
using System.Text;

namespace Amazon.Ses;

public static class QuotedPrintable
{
    public static string Encode(string text)
    {
        // RFC 2047.

        string encoded = EncodeContent(text);

        return !encoded.Equals(text, StringComparison.Ordinal)
            ? $"=?utf-8?Q?{encoded}?="
            : text;
    }

    public static string Decode(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        if (text.IndexOf(' ') > 0)
        {
            text = text.Replace(" ", string.Empty);
        }

        using var a = Attachment.CreateAttachmentFromString(string.Empty, text);

        return a.Name!;
    }

    // Based on https://stackoverflow.com/questions/11793734/code-for-encode-decode-quotedprintable

    private static string EncodeContent(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        byte[] rentedBuffer = ArrayPool<byte>.Shared.Rent(Encoding.UTF8.GetMaxByteCount(value.Length));

        ReadOnlySpan<byte> bytes = rentedBuffer.AsSpan(0, Encoding.UTF8.GetBytes(value, rentedBuffer));

        var sb = new StringBuilder();

        foreach (byte v in bytes)
        {
            // The following are not required to be encoded:
            // - Tab (ASCII 9)
            // - Space (ASCII 32)
            // - Characters 33 to 126, except for the equal sign (61).

            if ((v == 9) || ((v >= 32) && (v <= 60)) || ((v >= 62) && (v <= 126)))
            {
                sb.Append(Convert.ToChar(v));
            }
            else
            {
                sb.Append($"={v:X2}");
            }
        }

        ArrayPool<byte>.Shared.Return(rentedBuffer);

        char lastChar = sb[^1];

        if (char.IsWhiteSpace(lastChar))
        {
            sb.Remove(sb.Length - 1, 1);
            sb.Append('=');
            sb.Append(((int)lastChar).ToString("X2"));
        }

        return sb.ToString();
    }
}