using System.Text.Encodings.Web;

namespace Amazon.DynamoDb.Models;

using static DbValueType;

public readonly ref struct AttributeWriter(TextWriter writer)
{
    private readonly TextWriter _writer = writer;

    public void WriteProperty(string name, in DbValue value)
    {
        _writer.Write('"');
        JavaScriptEncoder.Default.Encode(_writer, name);
        _writer.Write('"');

        _writer.Write(':');

        WriteDbValue(value);
    }
         
    public void WriteDbValue(in DbValue value)
    {
        switch (value.Kind)
        {
            case S    : WriteString(value.ToString());              break;
            case B    : WriteValue("B", value.ToString());          break;
            case BOOL : WriteBool(value.ToBoolean());               break;
            case BS   : WriteSet("BS", value.ToSet<byte[]>());      break;
            case SS   : WriteSet("SS", value.ToSet<string>());      break;
            case NS   : WriteSet("NS", value.ToSet<string>());      break;
            case L    : WriteList((DbValue[])value.Value);          break;
            case N    : WriteValue("N", value.ToString());          break;
            case M    : WriteMap((AttributeCollection)value.Value); break;
            default   : throw new Exception($"Invalid type. Was {value.Kind}");
        }
    }

    public void WriteMap(AttributeCollection map)
    {
        int i = 0;

        _writer.Write(@"{""M"":{");

        foreach (var property in map)
        {
            if (i != 0) _writer.Write(',');

            WriteProperty(property.Key, property.Value);

            i++;
        }

        _writer.Write("}}");
    }

    private void WriteList(DbValue[] values)
    {
        // { "L":[] }
        _writer.Write(@"{""L"":[");

        for (int i = 0; i < values.Length; i++)
        {
            if (i != 0) _writer.Write(',');

            ref DbValue value = ref values[i];

            WriteDbValue(value);
        }

        _writer.Write("]}");
    }

    private void WriteSet(string type, IEnumerable<object> values)
    {
        // { "SS":[] }
        _writer.Write("{\"");
        _writer.Write(type);
        _writer.Write(@""":[");

        int i = 0;

        foreach (var value in values)
        {
            if (i != 0) _writer.Write(',');

            _writer.Write('"');

            if (type is "S")
            {
                JavaScriptEncoder.Default.Encode(_writer, value.ToString()!);
            }
            else
            {
                _writer.Write(value.ToString());
            }

            _writer.Write('"');

            i++;
        }

        _writer.Write("]}");
    }

    private void WriteBool(bool value)
    {
        _writer.Write("{\"BOOL\":");
        _writer.Write(value ? "true" : "false");
        _writer.Write('}');
    }

    private void WriteString(string value)
    {
        _writer.Write("{\"S\":");
        _writer.Write('"');
        JavaScriptEncoder.Default.Encode(_writer, value);
        _writer.Write('"');
        _writer.Write('}');
    }

    private void WriteValue(string type, string value)
    {
        _writer.Write("{\"");
        _writer.Write(type);
        _writer.Write("\":\"");
        _writer.Write(value);
        _writer.Write("\"}");
    }
}