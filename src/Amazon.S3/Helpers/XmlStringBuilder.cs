using System.Text;

namespace Amazon.S3.Helpers;

internal ref struct XmlStringBuilder(bool pretty)
{
    private ValueStringBuilder _sb = new(1024);
    private bool _pretty = pretty;
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

    public void WriteTag(string tag, string value)
    {
        if (_sb.Length > 0 && _pretty)
        {
            _sb.Append('\n');
            Indent();
        }

        _sb.Append('<');
        _sb.Append(tag);
        _sb.Append('>');

        _sb.Append(value);

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
