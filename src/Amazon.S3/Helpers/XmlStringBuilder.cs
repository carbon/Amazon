using System.Buffers;
using System.Globalization;
using System.Text;

namespace Amazon.S3.Helpers;

internal ref struct XmlStringBuilder(bool pretty)
{
    private static readonly SearchValues<char> s_escapedChars = SearchValues.Create("&<>\r");

    private ValueStringBuilder _sb = new(1024);
    private readonly bool _pretty = pretty;
    private int _level = 0;

    public void WriteTagStart(string tag)
    {
        if (_sb.Length > 0 && _pretty)
        {
            _sb.Append('\n');
            Indent();
        }

        _sb.Append('<');
        _sb.Append(tag);
        _sb.Append('>');

        _level++;
    }

    public void WriteTag(string tag, ReadOnlySpan<char> value)
    {
        WriteLeafStart(tag);

        AppendEscaped(value);

        WriteLeafEnd(tag);
    }

    public void WriteTag(string tag, int value)
    {
        const int maxLength = 11; // -2147483648

        WriteLeafStart(tag);

        value.TryFormat(_sb.AppendSpan(maxLength), out int written, provider: CultureInfo.InvariantCulture);

        _sb.Length -= maxLength - written;

        WriteLeafEnd(tag);
    }

    private void AppendEscaped(ReadOnlySpan<char> value)
    {
        int index;

        while ((index = value.IndexOfAny(s_escapedChars)) > -1)
        {
            _sb.Append(value[..index]);

            _sb.Append(value[index] switch {
                '&' => "&amp;",
                '<' => "&lt;",
                '>' => "&gt;",
                _   => "&#xD;" // XML parsers normalize a literal carriage return to a line feed
            });

            value = value[(index + 1)..];
        }

        _sb.Append(value);
    }

    private void WriteLeafStart(string tag)
    {
        if (_sb.Length > 0 && _pretty)
        {
            _sb.Append('\n');
            Indent();
        }

        _sb.Append('<');
        _sb.Append(tag);
        _sb.Append('>');
    }

    private void WriteLeafEnd(string tag)
    {
        _sb.Append("</");
        _sb.Append(tag);
        _sb.Append('>');
    }

    public void WriteTagEnd(string tag)
    {
        _level--;

        if (_sb.Length > 0 && _pretty)
        {
            _sb.Append('\n');
            Indent();
        }

        _sb.Append("</");
        _sb.Append(tag);
        _sb.Append('>');
    }

    public void Indent()
    {
        if (!_pretty) return;

        for (int i = 0; i < _level; i++)
        {
            _sb.Append("  ");
        }
    }

    public override string ToString()
    {
        return _sb.ToString();
    }

    public byte[] ToUtf8Bytes()
    {
        ReadOnlySpan<char> chars = _sb.AsSpan();

        byte[] bytes = new byte[Encoding.UTF8.GetByteCount(chars)];

        Encoding.UTF8.GetBytes(chars, bytes);

        _sb.Dispose();

        return bytes;
    }

    public void Dispose()
    {
        _sb.Dispose();
    }
}


/*
<?xml version="1.0" encoding="UTF-8"?>
<Delete>
    <Quiet>true</Quiet>
    <Object>
         <Key>Key</Key>
         <VersionId>VersionId</VersionId>
    </Object>
    <Object>
         <Key>Key</Key>
    </Object>
    ...
</Delete>	
*/
