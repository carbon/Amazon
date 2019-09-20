using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;

using Carbon.Json;

namespace Amazon.DynamoDb.Models
{
    using static DbValueType;

    public readonly ref struct AttributeWriter
    {
        private readonly TextWriter writer;

        public AttributeWriter(TextWriter writer)
        {
            this.writer = writer;
        }

        public void WriteJsonObject(JsonObject obj)
        {
            int i = 0;

            writer.Write('{');
            
            foreach (var property in obj)
            {
                if (i != 0) writer.Write(',');

                writer.Write('"');
                JavaScriptEncoder.Default.Encode(writer, property.Key);
                writer.Write('"');
                
                writer.Write(':');

                WriteXNode(property.Value);

                i++;
            }

            writer.Write('}');
        }

        public void WriteProperty(string name, in DbValue value)
        {
            writer.Write('"');
            JavaScriptEncoder.Default.Encode(writer, name);
            writer.Write('"');

            writer.Write(':');

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
                default   : throw new Exception("Invalid type:" + value.Kind);
            }
        }
      

       
        public void WriteXNode(JsonNode value)
        {
            switch (value.Type)
            {
                case JsonType.Number  : WriteValue("N", value.ToString());     break;
                case JsonType.String  : WriteString(value.ToString());         break;
                case JsonType.Binary  : WriteValue("B", value.ToString());     break;
                case JsonType.Boolean : WriteBool(((JsonBoolean)value).Value); break;

                case JsonType.Array:
                    var array = (JsonArray)value;

                    if (array.IsSet && array.ElementType != null)
                    {
                        switch (array.ElementType.Value)
                        {
                            case JsonType.Number: WriteSet("NS", array); break;
                            case JsonType.Binary: WriteSet("BS", array); break;
                            case JsonType.String: WriteSet("SS", array); break;
                        }
                    }
                    else
                    {
                        WriteList(array);
                    }

                    break;

                case JsonType.Object:
                    {
                        writer.Write(@"{""M"":");

                        WriteJsonObject((JsonObject)value);

                        writer.Write('}');

                        break;
                    }
            }
        }

        public void WriteMap(AttributeCollection map)
        {
            int i = 0;

            writer.Write(@"{""M"":{");

            foreach (var property in map)
            {
                if (i != 0) writer.Write(',');

                WriteProperty(property.Key, property.Value);

                i++;
            }

            writer.Write("}}");
        }

        private void WriteList(DbValue[] values)
        {
            // { "L":[] }
            writer.Write(@"{""L"":[");

            for (int i = 0; i < values.Length; i++)
            {
                if (i != 0) writer.Write(',');

                ref DbValue value = ref values[i];

                WriteDbValue(value);
            }

            writer.Write(@"]}");
        }

        private void WriteList(IEnumerable<JsonNode> values)
        {
            // { "SS":[] }
            writer.Write(@"{""L"":[");

            int i = 0;

            foreach (var value in values)
            {
                if (i != 0) writer.Write(',');

                WriteXNode(value);

                i++;
            }

            writer.Write(@"]}");
        }

        private void WriteSet(string type, IEnumerable<object> values)
        {
            // { "SS":[] }
            writer.Write(@"{""");
            writer.Write(type);
            writer.Write(@""":[");

            int i = 0;

            foreach (var value in values)
            {
                if (i != 0) writer.Write(',');

                writer.Write('"');

                if (type == "S")
                {
                    JavaScriptEncoder.Default.Encode(writer, value.ToString());
                }
                else
                {
                    writer.Write(value.ToString());
                }

                writer.Write('"');

                i++;
            }

            writer.Write(@"]}");
        }

        private void WriteBool(bool value)
        {
            writer.Write(@"{""BOOL"":");
            writer.Write(value ? "true" : "false");
            writer.Write('}');
        }

        private void WriteString(string value)
        {
            writer.Write(@"{""S"":");
            writer.Write('"');
            JavaScriptEncoder.Default.Encode(writer, value);
            writer.Write('"');
            writer.Write('}');
        }

        private void WriteValue(string type, string value)
        {
            writer.Write(@"{""");
            writer.Write(type);
            writer.Write(@""":""");
            writer.Write(value);
            writer.Write(@"""}");
        }
    }
}