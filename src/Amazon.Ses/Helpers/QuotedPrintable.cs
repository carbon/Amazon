using System.Buffers;
using System.Net.Mail;
using System.Text;

namespace Amazon.Ses;

// RFC 2047.
public static class QuotedPrintable
{
    public static string Encode(string text)
    {
        // TODO: handle > 76 length

        string encoded = EncodeContent(text);

        return !encoded.Equals(text, StringComparison.Ordinal)
            ? $"=?utf-8?Q?{encoded}?="
            : text;
    }

    public static string Decode(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        if (text.Contains(' '))
        {
            text = text.Replace(" ", string.Empty);
        }

        using var a = Attachment.CreateAttachmentFromString(string.Empty, text);

        return a.Name!;
    }

    public static int GetEncodedCharCount(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return 0;
        }

        int maxByteCount = Encoding.UTF8.GetMaxByteCount(value.Length);
        byte[]? rentedBuffer = null;

        int count = 0;

        Span<byte> buffer = maxByteCount < 128
            ? stackalloc byte[128]
            : (rentedBuffer = ArrayPool<byte>.Shared.Rent(maxByteCount));

        ReadOnlySpan<byte> bytes = buffer[..Encoding.UTF8.GetBytes(value, buffer)];

        for (int i = 0; i < bytes.Length; i++)
        {
            var b = bytes[i];

            if (b is 32) // ' '
            {
                if (i == bytes.Length - 1)
                {
                    count += 3;
                }
                else
                {
                    count += 1;
                }
            }
            else if ((b < 33 || b > 126 || b == '=' || b == '?'))
            {
                count += 3;
            }
            else
            {
                count += 1;
            }
        }

        if (rentedBuffer != null)
        {
            ArrayPool<byte>.Shared.Return(rentedBuffer);
        }

        return count;
    }

    private static string EncodeContent(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        int maxByteCount = Encoding.UTF8.GetMaxByteCount(value.Length);
        byte[]? rentedBuffer = null;

        Span<byte> buffer = maxByteCount < 128
            ? stackalloc byte[128]
            : (rentedBuffer = ArrayPool<byte>.Shared.Rent(maxByteCount));

        ReadOnlySpan<byte> bytes = buffer[..Encoding.UTF8.GetBytes(value, buffer)];

        var sb = new ValueStringBuilder(bytes.Length * 2);

        for (int i = 0; i < bytes.Length; i++)
        {
            var b = bytes[i];

            if (b is 32) // ' '
            {
                // spaces can either be encoded as either:
                // =20 (the hexadecimal value of the space character in ASCII)
                // an underscore (_)

                if (i == bytes.Length - 1)
                {
                    // if a space appears at the end of a line, it must be encoded as =20 to prevent it from being trimmed or misinterpreted.
                    sb.Append("=20");
                }
                else
                {
                    sb.Append('_');
                }
            }           
            else if ((b < 33 || b > 126 || b == '=' || b == '?'))
            {
                sb.Append('=');
                sb.Append(ToHexChar((b >> 4) & 0xF));
                sb.Append(ToHexChar(b & 0xF));
            }
            else
            {
                sb.Append(Convert.ToChar(b));
            }
        }

        if (rentedBuffer != null)
        {
            ArrayPool<byte>.Shared.Return(rentedBuffer);
        }

        return sb.ToString();
    }

    private static char ToHexChar(int value)
    {
        if (value < 10) return (char)('0' + value);
        return (char)('A' + (value - 10));
    }
}