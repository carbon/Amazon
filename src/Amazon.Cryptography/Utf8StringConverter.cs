using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Amazon.Cryptography;

public sealed partial class Utf8StringConverter : JsonConverter<Utf8String>
{
    public override Utf8String Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (!reader.HasValueSequence && !reader.ValueIsEscaped)
        {
            return new Utf8String(reader.ValueSpan.ToArray());
        }

        return ReadComplex(ref reader);
    }

    public override Utf8String ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Read(ref reader, typeToConvert, options);
    }

    private Utf8String ReadComplex(ref Utf8JsonReader reader)
    {
        int valueLength = reader.HasValueSequence
           ? checked((int)reader.ValueSequence.Length)
           : reader.ValueSpan.Length;

        Span<byte> buffer = valueLength <= 128
            ? stackalloc byte[128]
            : new byte[valueLength];

        int bytesRead = reader.CopyString(buffer);

        return new Utf8String(buffer[..bytesRead].ToArray());
    }

    public override void Write(Utf8JsonWriter writer, Utf8String value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Utf8Bytes);
    }

    public override void WriteAsPropertyName(Utf8JsonWriter writer, [DisallowNull] Utf8String value, JsonSerializerOptions options)
    {
        writer.WritePropertyName(value.Utf8Bytes);
    }
}