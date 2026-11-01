using System.Net;
using System.Text.Json;

using Carbon.Data.Expressions;

namespace Amazon.DynamoDb.Tests;

public class DbValueConversionTests
{
    private static object RoundTrip<T>(T value)
    {
        var converter = DbValueConverterFactory.Get(typeof(T));

        return converter.ToObject(converter.FromObject(value, null), null);
    }

    [Fact]
    public void NoConverterForUnknownType()
    {
        Assert.Throws<Exception>(() => DbValueConverterFactory.Get(typeof(DbValueConversionTests)));
    }

    [Fact]
    public void Numbers_SerializeAsStrings()
    {
        Assert.Equal("""{"N":"5"}""", JsonSerializer.Serialize(new DbValue(5)));
        Assert.Equal("""{"N":"-5"}""", JsonSerializer.Serialize(new DbValue(-5L)));
        Assert.Equal("""{"N":"1.5"}""", JsonSerializer.Serialize(new DbValue(1.5d)));
        Assert.Equal("""{"N":"79228162514264337593543950335"}""", JsonSerializer.Serialize(new DbValue(decimal.MaxValue)));
        Assert.Equal("""{"N":"12"}""", JsonSerializer.Serialize(new DbValue("12", DbValueType.N)));
    }

    [Fact]
    public void Numbers_SerializeUsingInvariantCulture()
    {
        var culture = System.Globalization.CultureInfo.CurrentCulture;

        try
        {
            System.Globalization.CultureInfo.CurrentCulture = new System.Globalization.CultureInfo("de-DE");

            Assert.Equal("""{"N":"1.5"}""", JsonSerializer.Serialize(new DbValue(1.5d)));
            Assert.Equal("""{"NS":["1.5"]}""", JsonSerializer.Serialize(new DbValue(new double[] { 1.5d })));
        }
        finally
        {
            System.Globalization.CultureInfo.CurrentCulture = culture;
        }
    }

    [Fact]
    public void NumberSets_Serialize()
    {
        Assert.Equal("""{"NS":["1","2"]}""", JsonSerializer.Serialize(new DbValue(new short[] { 1, 2 })));
        Assert.Equal("""{"NS":["1","2"]}""", JsonSerializer.Serialize(new DbValue(new int[] { 1, 2 })));
        Assert.Equal("""{"NS":["1","2"]}""", JsonSerializer.Serialize(new DbValue(new long[] { 1, 2 })));
        Assert.Equal("""{"NS":["1.5"]}""", JsonSerializer.Serialize(new DbValue(new float[] { 1.5f })));
        Assert.Equal("""{"NS":["1","2"]}""", JsonSerializer.Serialize(new DbValue(new string[] { "1", "2" }, DbValueType.NS)));
    }

    [Fact]
    public void BinarySets_SerializeFromBytes()
    {
        var value = new DbValue(new byte[][] { [1, 2, 3], [4, 5, 6] });

        Assert.Equal("""{"BS":["AQID","BAUG"]}""", JsonSerializer.Serialize(value));
    }

    [Fact]
    public void BinarySets_SerializeFromBase64Strings()
    {
        var value = new DbValue(new string[] { "AQID", "BAUG" }, DbValueType.BS);

        Assert.Equal("""{"BS":["AQID","BAUG"]}""", JsonSerializer.Serialize(value));
    }

    [Fact]
    public void BinarySets_RoundTripThroughJson()
    {
        const string json = """{"BS":["AQID","BAUG"]}""";

        Assert.Equal(json, JsonSerializer.Serialize(JsonSerializer.Deserialize<DbValue>(json)));
    }

    [Fact]
    public void BinarySets_RoundTripInsideAttributeCollection()
    {
        const string json = """{"id":{"N":"1"},"data":{"BS":["AQID"]}}""";

        Assert.Equal(json, JsonSerializer.Serialize(JsonSerializer.Deserialize<AttributeCollection>(json)));
    }

    [Fact]
    public void BinarySets_PreserveBase64SpecialCharacters()
    {
        // 0xFB 0xEF 0xFF encodes to "++//", which the JSON writer may escape
        byte[] bytes = [0xFB, 0xEF, 0xFF];

        var fromBytes = new DbValue(new byte[][] { bytes });
        var fromJson  = JsonSerializer.Deserialize<DbValue>("""{"BS":["++//"]}""");

        foreach (var value in new[] { fromBytes, fromJson })
        {
            using var json = JsonDocument.Parse(JsonSerializer.Serialize(value));

            var item = Assert.Single(json.RootElement.GetProperty("BS").EnumerateArray());

            Assert.Equal(bytes, item.GetBytesFromBase64());
        }
    }

    [Fact]
    public void ListsAndMaps_Serialize()
    {
        var map = new AttributeCollection { { "a", 1 }, { "b", new DbValue(true) } };

        Assert.Equal("""{"a":{"N":"1"},"b":{"BOOL":true}}""", JsonSerializer.Serialize(map));
        Assert.Equal("""{"L":[{"N":"1"},{"S":"a"}]}""", JsonSerializer.Serialize(new DbValue(new DbValue[] { new(1), new("a") })));
        Assert.Equal("""{"L":[{"N":"1"}]}""", JsonSerializer.Serialize(new DbValue(new List<DbValue> { new(1) })));
    }

    [Fact]
    public void Booleans_Deserialize()
    {
        Assert.True(JsonSerializer.Deserialize<DbValue>("""{"BOOL":true}""").ToBoolean());
        Assert.False(JsonSerializer.Deserialize<DbValue>("""{"BOOL":false}""").ToBoolean());
    }

    [Fact]
    public void Decimal_RoundTrips() => Assert.Equal(1.5m, RoundTrip(1.5m));

    [Fact]
    public void Double_RoundTrips() => Assert.Equal(1.5d, RoundTrip(1.5d));

    [Fact]
    public void UInt32_RoundTrips() => Assert.Equal(5u, RoundTrip(5u));

    [Fact]
    public void Guid_RoundTrips()
    {
        var value = Guid.NewGuid();

        Assert.Equal(value, RoundTrip(value));
    }

    [Fact]
    public void Uri_RoundTrips()
    {
        var value = new Uri("https://example.com/a");

        Assert.Equal(value, RoundTrip(value));
    }

    [Fact]
    public void Version_RoundTrips()
    {
        var value = new Version(1, 2, 3);

        Assert.Equal(value, RoundTrip(value));
    }

    [Fact]
    public void IPAddress_RoundTrips()
    {
        var value = IPAddress.Parse("10.0.0.1");

        Assert.Equal(value, RoundTrip(value));
    }

    [Fact]
    public void IPAddress_ParsesText()
    {
        var converter = DbValueConverterFactory.Get(typeof(IPAddress));

        Assert.Equal(IPAddress.Parse("10.0.0.1"), converter.ToObject(new DbValue("10.0.0.1"), null));
        Assert.Throws<Exception>(() => converter.ToObject(new DbValue(1), null));
    }

    [Fact]
    public void DateTimeOffset_UsesUnixSeconds()
    {
        var value = DateTimeOffset.FromUnixTimeSeconds(1497212690);

        Assert.Equal(value, RoundTrip(value));
        Assert.Equal(1497212690L, DbValueConverterFactory.Get(typeof(DateTimeOffset)).FromObject(value, null).Value);
    }

    [Fact]
    public void DateTime_UsesUnixSeconds()
    {
        var value = DateTimeOffset.FromUnixTimeSeconds(1497212690).UtcDateTime;

        Assert.Equal(value, RoundTrip(value));
    }

    [Fact]
    public void TimeSpan_RoundTrips()
    {
        var value = TimeSpan.FromMinutes(5);

        Assert.Equal(value, RoundTrip(value));
    }

    [Fact]
    public void TimeSpan_RoundTripsLongDurations()
    {
        var value = TimeSpan.FromDays(30);

        Assert.Equal(value, RoundTrip(value));
    }

    [Fact]
    public void HashSets_RoundTrip()
    {
        Assert.Equal(new HashSet<string> { "a", "b" }, RoundTrip(new HashSet<string> { "a", "b" }));
        Assert.Equal(new HashSet<int> { 1, 2 }, RoundTrip(new HashSet<int> { 1, 2 }));
    }

    [Fact]
    public void Arrays_RoundTrip()
    {
        Assert.Equal(new[] { "a", "b" }, RoundTrip(new[] { "a", "b" }));
        Assert.Equal(new[] { 1, 2 }, RoundTrip(new[] { 1, 2 }));
    }

    [Theory]
    [InlineData(DbValueType.B, "B")]
    [InlineData(DbValueType.BOOL, "BOOL")]
    [InlineData(DbValueType.BS, "BS")]
    [InlineData(DbValueType.L, "L")]
    [InlineData(DbValueType.M, "M")]
    [InlineData(DbValueType.N, "N")]
    [InlineData(DbValueType.NS, "NS")]
    [InlineData(DbValueType.S, "S")]
    [InlineData(DbValueType.SS, "SS")]
    public void Kinds_SerializeWithTheirWireName(DbValueType kind, string name)
    {
        object value = kind switch {
            DbValueType.B    => new byte[] { 1 },
            DbValueType.BOOL => true,
            DbValueType.BS   => new[] { new byte[] { 1 } },
            DbValueType.L    => new[] { new DbValue(1) },
            DbValueType.M    => new AttributeCollection { { "a", new DbValue(1) } },
            DbValueType.N    => 1,
            DbValueType.NS   => new[] { 1 },
            DbValueType.S    => "a",
            _                => new[] { "a" }
        };

        using var json = JsonDocument.Parse(JsonSerializer.Serialize(new DbValue(value, kind)));

        Assert.Equal(name, json.RootElement.EnumerateObject().Single().Name);
    }

    [Fact]
    public void ObjectConstructor_RejectsEmptyStringsAndUnknownTypes()
    {
        Assert.Throws<ArgumentException>(() => new DbValue((object)""));
        Assert.Throws<Exception>(() => new DbValue((object)new DbValueConversionTests()));
        Assert.Throws<Exception>(() => new DbValue((object)new[] { new DbValueConversionTests() }));
    }

    [Fact]
    public void ObjectConstructor_ConvertsPrimitives()
    {
        Assert.Equal(DbValueType.SS, new DbValue((object)new[] { "a" }).Kind);
        Assert.Equal(DbValueType.NS, new DbValue((object)new[] { 1 }).Kind);
        Assert.Equal(DbValueType.B, new DbValue((object)new byte[] { 1 }).Kind);
        Assert.Equal(1, new DbValue((object)true).Value);
        Assert.Equal(1497212690L, new DbValue((object)DateTimeOffset.FromUnixTimeSeconds(1497212690).UtcDateTime).Value);
        Assert.Equal(DbValueType.M, new DbValue((object)new AttributeCollection()).Kind);
    }

    [Fact]
    public void ObjectConstructor_UnwrapsDbValue()
    {
        var value = new DbValue((object)new DbValue("a"));

        Assert.Equal(DbValueType.S, value.Kind);
        Assert.Equal("a", value.Value);
    }

    [Fact]
    public void ObjectConstructor_UsesConverterValueForDateTimeOffset()
    {
        var value = new DbValue((object)DateTimeOffset.FromUnixTimeSeconds(1497212690));

        Assert.Equal(1497212690L, value.Value);
        Assert.Equal("""{"N":"1497212690"}""", JsonSerializer.Serialize(value));
    }

    [Fact]
    public void ExpressionValues_SerializeForSupportedTypes()
    {
        var expression = DynamoExpression.Conjunction(Expression.Eq("a", "x"), Expression.Eq("b", 1));

        Assert.Equal("a = :v0 and b = :v1", expression.Text);
    }

    [Fact]
    public void InvalidJsonIsRejected()
    {
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<DbValue>("1"));
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<DbValue>("""{"X":"1"}"""));
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<DbValue>("""{"SS":"a"}"""));
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<DbValue>("""{"L":"a"}"""));
    }

    [Fact]
    public void ReadsListsAndMaps()
    {
        var value = JsonSerializer.Deserialize<DbValue>("""{"L":[{"N":"1"},{"M":{"a":{"S":"b"}}},{"BOOL":false}]}""");

        var items = value.ToArray<DbValue>();

        Assert.Equal(DbValueType.L, value.Kind);
        Assert.Equal(1, items[0].ToInt());
        Assert.Equal(DbValueType.M, items[1].Kind);
        Assert.False(items[2].ToBoolean());
    }

    [Fact]
    public void ReadsNull()
    {
        var item = JsonSerializer.Deserialize<AttributeCollection>("""{"id":{"N":"1"},"deleted":{"NULL":true}}""");

        Assert.Equal(DbValueType.NULL, item["deleted"].Kind);
    }

    [Fact]
    public void WritesNull()
    {
        Assert.Equal("""{"NULL":true}""", JsonSerializer.Serialize(DbValue.Null));
    }

    [Fact]
    public void ObjectConstructor_UsesConverterValueForGuid()
    {
        var guid = Guid.NewGuid();

        var value = new DbValue((object)guid);

        Assert.Equal(DbValueConverterFactory.Get(typeof(Guid)).FromObject(guid, null).Kind, value.Kind);

        var json = JsonSerializer.Serialize(value);

        Assert.Equal(guid, DbValueConverterFactory.Get(typeof(Guid)).ToObject(JsonSerializer.Deserialize<DbValue>(json), null));
    }

    [Fact]
    public void ReadsNullInsideListsAndMaps()
    {
        var value = JsonSerializer.Deserialize<DbValue>("""{"L":[{"NULL":true},{"M":{"a":{"NULL":true}}}]}""");

        var items = value.ToArray<DbValue>();

        Assert.Equal(DbValueType.NULL, items[0].Kind);
        Assert.Equal(DbValueType.NULL, ((AttributeCollection)items[1].Value)["a"].Kind);
    }
}
