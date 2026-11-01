using System.Buffers;
using System.Text;
using System.Text.Json;

using Amazon.Cryptography.Buffers;

namespace Amazon.Cryptography.Tests;

public class EncryptionContextSerializationTests
{
    [Fact]
    public void Empty_SerializesToNothing()
    {
        var context = new EncryptionContext();
        var writer = new ArrayBufferWriter<byte>();
        var destination = new byte[] { 0xAA };

        context.Serialize(writer);
        context.Serialize(destination);

        Assert.Equal(0, context.GetLength());
        Assert.Empty(context.Serialize());
        Assert.Equal(0, writer.WrittenCount);
        Assert.Equal(0xAA, destination[0]);
    }

    [Fact]
    public void Deserialize_Empty_ReturnsEmpty()
    {
        Assert.Empty(EncryptionContext.Deserialize([]));
    }

    [Fact]
    public void AllSerializers_ProduceTheDocumentedLayout()
    {
        var context = new EncryptionContext {
            { "a"u8, "bc"u8 },
            { "d"u8, "é"u8 } // two bytes in UTF-8: lengths are bytes, not characters
        };

        byte[] expected = [
            0x00, 0x02,
            0x00, 0x01, (byte)'a', 0x00, 0x02, (byte)'b', (byte)'c',
            0x00, 0x01, (byte)'d', 0x00, 0x02, 0xC3, 0xA9
        ];

        var writer = new ArrayBufferWriter<byte>();
        var span = new byte[expected.Length];

        context.Serialize(writer);
        context.Serialize(span);

        Assert.Equal(expected.Length, context.GetLength());
        Assert.Equal(expected, context.Serialize());
        Assert.Equal(expected, span);
        Assert.Equal(expected.AsSpan(), writer.WrittenSpan);
    }

    [Fact]
    public void RoundTrips()
    {
        var context = new EncryptionContext {
            { "accountId"u8, "1"u8 },
            { "empty"u8, ""u8 },
            { "name"u8, "naïve ☃"u8 }
        };

        var result = EncryptionContext.Deserialize(context.Serialize());

        Assert.Equal(3, result.Count);
        Assert.Equal("1", result["accountId"u8.ToArray()]);
        Assert.Equal("", result["empty"u8.ToArray()]);
        Assert.Equal("naïve ☃", result["name"u8.ToArray()]);
    }

    [Fact]
    public void Serialize_RejectsDestinationThatIsTooSmall()
    {
        var context = new EncryptionContext { { "a"u8, "b"u8 } };

        Assert.ThrowsAny<ArgumentException>(() => context.Serialize(new byte[context.GetLength() - 1]));
    }

    [Theory]
    [InlineData("00")] // partial count
    [InlineData("0001")] // missing pair
    [InlineData("000100")] // partial key length
    [InlineData("00010005")] // key length exceeds data
    [InlineData("0001000161")] // missing value length
    [InlineData("000100016100056162")] // value length exceeds data
    public void Deserialize_RejectsMalformedInput(string hex)
    {
        var data = Convert.FromHexString(hex);

        Assert.ThrowsAny<ArgumentException>(() => EncryptionContext.Deserialize(data));
    }

    [Fact]
    public void Serialize_SortsByKey()
    {
        var context = new EncryptionContext {
            { "b"u8, "1"u8 },
            { "a"u8, "2"u8 }
        };

        Assert.Equal("00020001610001320001620001" + "31", Convert.ToHexStringLower(context.Serialize()));
    }

    [Fact]
    public void Serialize_ComparesKeysAsUnsignedBytes_AndLeavesInsertionOrderAlone()
    {
        var context = new EncryptionContext {
            { "é"u8, "1"u8 }, // 0xC3 sorts after 'z'
            { "z"u8, "2"u8 }
        };

        var expected = "0002" + "00017a000132" + "0002c3a9000131";
        var writer = new ArrayBufferWriter<byte>();
        var span = new byte[context.GetLength()];

        context.Serialize(writer);
        context.Serialize(span);

        Assert.Equal(expected, Convert.ToHexStringLower(context.Serialize()));
        Assert.Equal(expected, Convert.ToHexStringLower(writer.WrittenSpan));
        Assert.Equal(expected, Convert.ToHexStringLower(span));
        Assert.Equal("é", context.GetAt(0).Key.ToString());
    }

    [Fact]
    public void Deserialize_RejectsUnsortedKeys()
    {
        var data = Convert.FromHexString("0002" + "0001620001" + "31" + "0001610001" + "32");

        Assert.Throws<FormatException>(() => EncryptionContext.Deserialize(data));
    }

    [Theory]
    [InlineData("0001" + "0001FF" + "000131")] // invalid key
    [InlineData("0001" + "000161" + "0001C3")] // value ends mid-sequence
    [InlineData("0001" + "000161" + "0002C0AF")] // overlong encoding
    public void Deserialize_RejectsInvalidUtf8(string hex)
    {
        var data = Convert.FromHexString(hex);

        Assert.Throws<FormatException>(() => EncryptionContext.Deserialize(data));
    }

    [Fact]
    public void Deserialize_RejectsZeroCount()
    {
        Assert.Throws<FormatException>(() => EncryptionContext.Deserialize([0x00, 0x00]));
    }

    [Fact]
    public void Deserialize_RejectsDuplicateKeys()
    {
        var data = Convert.FromHexString("0002" + "0001610001" + "31" + "0001610001" + "32");

        Assert.ThrowsAny<Exception>(() => EncryptionContext.Deserialize(data));
    }

    [Fact]
    public void Deserialize_RejectsTrailingBytes()
    {
        var data = Convert.FromHexString("0001" + "0001610001" + "31" + "FF");

        Assert.ThrowsAny<Exception>(() => EncryptionContext.Deserialize(data));
    }

    [Fact]
    public void GetLength_RejectsContextsThatDoNotFit()
    {
        var context = new EncryptionContext {
            { "a"u8, new byte[32_767] },
            { "b"u8, new byte[32_767] }
        };

        Assert.ThrowsAny<Exception>(() => context.GetLength());
    }

    [Fact]
    public void Serialize_RejectsValueThatDoesNotFit()
    {
        var context = new EncryptionContext { { "a"u8, new byte[70_000] } };

        Assert.ThrowsAny<Exception>(() => context.Serialize(new ArrayBufferWriter<byte>()));
    }
}

public class EncryptedDataKeyBehaviorTests
{
    private static EncryptedDataKey Key(string id = "id", string info = "info", byte b = 1)
    {
        return new EncryptedDataKey(Encoding.UTF8.GetBytes(id), Encoding.UTF8.GetBytes(info), [b]);
    }

    [Fact]
    public void Equality_ComparesEveryField()
    {
        Assert.True(Key() == Key());
        Assert.False(Key() != Key());
        Assert.True(Key().Equals((object)Key()));

        Assert.True(Key() != Key(id: "other"));
        Assert.True(Key() != Key(info: "other"));
        Assert.True(Key() != Key(b: 2));

        Assert.False(Key().Equals("not a key"));
        Assert.False(Key().Equals(null));
    }

    [Fact]
    public void EqualKeys_HaveEqualHashCodes()
    {
        Assert.Equal(Key().GetHashCode(), Key().GetHashCode());
    }

    [Theory]
    [InlineData("")]
    [InlineData("0001")] // provider id length exceeds data
    [InlineData("00016100")] // partial provider info length
    [InlineData("000161000169")] // missing ciphertext length
    [InlineData("0001610001690005AA")] // ciphertext length exceeds data
    public void Read_RejectsMalformedInput(string hex)
    {
        var data = Convert.FromHexString(hex);

        Assert.ThrowsAny<Exception>(() => {
            var reader = new BufferReader(data);

            EncryptedDataKey.Read(ref reader);
        });
    }
}

public class BufferReaderTests
{
    [Fact]
    public void ReadsBigEndianValues_AndTracksEof()
    {
        var reader = new BufferReader([0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x00, 0x01, 0x41, 0x00, 0x01, 0x42]);

        Assert.False(reader.IsEof);
        Assert.Equal(0x01, reader.ReadByte());
        Assert.Equal(0x0203, reader.ReadUInt16());
        Assert.Equal(0x04050607u, reader.ReadUInt32());
        Assert.Equal("A", reader.ReadUtf8String().ToString());
        Assert.Equal([0x42], reader.ReadUInt16PrefixedBytes().ToArray());
        Assert.True(reader.IsEof);
    }

    [Fact]
    public void Reads_PastEnd_Throw()
    {
        Assert.ThrowsAny<Exception>(() => new BufferReader([]).ReadByte());
        Assert.ThrowsAny<Exception>(() => new BufferReader([1]).ReadUInt16());
        Assert.ThrowsAny<Exception>(() => new BufferReader([1, 2, 3]).ReadUInt32());
        Assert.ThrowsAny<Exception>(() => new BufferReader([1]).ReadBytes(2));
        Assert.ThrowsAny<Exception>(() => new BufferReader([1]).ReadBytes(-1));
        Assert.ThrowsAny<Exception>(() => new BufferReader([0, 2, 1]).ReadUInt16PrefixedBytes().ToArray());
    }
}

public class KeySizeTests
{
    [Fact]
    public void ConvertsBetweenBitsAndBytes()
    {
        Assert.Equal(32, KeySize.FromBitCount(256).ByteCount);
        Assert.Equal(256, KeySize.FromByteCount(32).BitCount);
    }
}

public class Utf8StringTests
{
    [Fact]
    public void ExposesBytesAndText()
    {
        Utf8String value = "é!"u8;

        Assert.Equal(3, value.Length);
        Assert.Equal([0xC3, 0xA9, 0x21], value.Utf8Bytes.ToArray());
        Assert.Equal([0xC3, 0xA9, 0x21], value.Value.ToArray());
        Assert.Equal([0x21], value.Slice(2).ToArray());
        Assert.Equal("é!", value.ToString());

        string text = value;

        Assert.Equal("é!", text);
    }

    [Fact]
    public void CopyTo_WritesBytes_AndRejectsShortDestination()
    {
        Utf8String value = new byte[] { 1, 2 };

        var destination = new byte[3];

        value.CopyTo(destination);

        Assert.Equal([1, 2, 0], destination);
        Assert.Throws<ArgumentException>(() => value.CopyTo(new byte[1]));
    }

    [Fact]
    public void Equality_IsByContent()
    {
        Utf8String a = new byte[] { 1, 2 };
        Utf8String b = new byte[] { 1, 2 };
        Utf8String c = new byte[] { 1, 3 };

        Assert.True(a.Equals(b));
        Assert.True(a.Equals((object)b));
        Assert.Equal(a.GetHashCode(), b.GetHashCode());

        Assert.False(a.Equals(c));
        Assert.False(a.Equals("not a Utf8String"));
    }
}

public class Utf8StringConverterTests
{
    [Theory]
    [InlineData("\"plain\"", "plain")]
    [InlineData("\"\"", "")]
    [InlineData("\"caf\\u00e9\"", "café")]
    [InlineData("\"line\\nbreak\"", "line\nbreak")]
    public void Read_DecodesStrings(string json, string expected)
    {
        Assert.Equal(expected, JsonSerializer.Deserialize<Utf8String>(json).ToString());
    }

    [Fact]
    public void Read_DecodesEscapedString_LongerThanStackBuffer()
    {
        var text = new string('x', 500);

        Assert.Equal(text + "\n", JsonSerializer.Deserialize<Utf8String>($"\"{text}\\n\"").ToString());
    }

    [Fact]
    public void Read_DecodesValueSpanningSegments()
    {
        var sequence = TwoSegments("\"split"u8.ToArray(), "-value\""u8.ToArray());

        var reader = new Utf8JsonReader(sequence);

        reader.Read();

        var result = new Utf8StringConverter().Read(ref reader, typeof(Utf8String), JsonSerializerOptions.Default);

        Assert.Equal("split-value", result.ToString());
    }

    [Fact]
    public void Write_ProducesJsonString()
    {
        Utf8String value = "a\"b"u8;

        Assert.Equal("\"a\\u0022b\"", JsonSerializer.Serialize(value));
    }

    [Fact]
    public void Dictionary_UsesPropertyNameConversion()
    {
        var result = JsonSerializer.Deserialize<Dictionary<Utf8String, Utf8String>>("""{"key":"value"}""")!;

        Assert.Equal("value", result["key"u8.ToArray()].ToString());
        Assert.Equal("""{"key":"value"}""", JsonSerializer.Serialize(result));
    }

    [Theory]
    [InlineData("123")]
    [InlineData("true")]
    [InlineData("null")]
    [InlineData("[]")]
    public void Read_RejectsNonStringTokens(string json)
    {
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<Utf8String>(json));
    }

    private static ReadOnlySequence<byte> TwoSegments(byte[] first, byte[] second)
    {
        var start = new Segment(first);
        var end = start.Append(second);

        return new ReadOnlySequence<byte>(start, 0, end, second.Length);
    }

    private sealed class Segment : ReadOnlySequenceSegment<byte>
    {
        public Segment(byte[] memory) => Memory = memory;

        public Segment Append(byte[] memory)
        {
            var next = new Segment(memory) { RunningIndex = RunningIndex + Memory.Length };

            Next = next;

            return next;
        }
    }
}
