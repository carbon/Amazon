using System.Globalization;
using System.Net;

namespace Amazon.CloudWatch.Tests;

public class PutMetricDataRequestTests
{
    private static async Task<RecordedRequest> SendAsync(PutMetricDataRequest request)
    {
        // Per the API reference, a successful call returns HTTP 200 with an empty body
        var handler = new FakeHandler(() => FakeHttp.Xml(""));

        await FakeHttp.CreateClient(handler).PutMetricDataAsync(request);

        return handler.Request;
    }

    private static MetricDatum Datum(string name = "buffers", params Dimension[]? dimensions) => new() {
        MetricName = name,
        Unit = "Bytes",
        Value = 23,
        Dimensions = dimensions
    };

    private static Dimension Dim(string name, string value) => new() { Name = name, Value = value };

    [Fact]
    public async Task EncodesDatumWithoutDimensions()
    {
        var sent = await SendAsync(new PutMetricDataRequest { Namespace = "TestNamespace" }.With(new MetricDatum {
            MetricName = "buffers",
            Unit = "Bytes",
            Value = 231434333
        }));

        Assert.Equal([
            new("Action", "PutMetricData"),
            new("Namespace", "TestNamespace"),
            new("MetricData.member.1.MetricName", "buffers"),
            new("MetricData.member.1.Unit", "Bytes"),
            new("MetricData.member.1.Value", "231434333"),
            new("Version", "2010-08-01")
        ], sent.Form);
    }

    [Fact]
    public async Task NumbersEachDatumFromOne()
    {
        var sent = await SendAsync(new PutMetricDataRequest { Namespace = "N" }.With(Datum("a"), Datum("b"), Datum("c")));

        Assert.Equal("a", sent["MetricData.member.1.MetricName"]);
        Assert.Equal("b", sent["MetricData.member.2.MetricName"]);
        Assert.Equal("c", sent["MetricData.member.3.MetricName"]);
    }

    [Fact]
    public async Task EmptyDimensionsDoNotAffectLaterData()
    {
        var sent = await SendAsync(new PutMetricDataRequest { Namespace = "N" }.With(Datum("a", []), Datum("b", [])));

        Assert.Equal("b", sent["MetricData.member.2.MetricName"]);
        Assert.DoesNotContain(sent.Form, f => f.Key.Contains("Dimensions"));
    }

    [Fact]
    public async Task OmitsUnsetTimestamp()
    {
        var sent = await SendAsync(new PutMetricDataRequest { Namespace = "N" }.With(Datum()));

        Assert.DoesNotContain(sent.Form, f => f.Key.EndsWith("Timestamp", StringComparison.Ordinal));
    }

    [Fact]
    public async Task EscapesNamespaceAndNames()
    {
        var sent = await SendAsync(new PutMetricDataRequest { Namespace = "My App/Prod&1" }.With(Datum("a+b c")));

        Assert.Equal("My App/Prod&1", sent["Namespace"]);
        Assert.Equal("a+b c", sent["MetricData.member.1.MetricName"]);
        Assert.Contains("Namespace=My+App%2FProd%261", sent.Body);
    }

    [Fact]
    public async Task EmptyRequestSendsNoMetricData()
    {
        var sent = await SendAsync(new PutMetricDataRequest { Namespace = "N" });

        Assert.DoesNotContain(sent.Form, f => f.Key.StartsWith("MetricData", StringComparison.Ordinal));
    }

    [Fact]
    public async Task ThrowsOnErrorResponse()
    {
        var handler = new FakeHandler(() => FakeHttp.Xml("<ErrorResponse><Error><Code>InvalidParameterValue</Code></Error></ErrorResponse>", HttpStatusCode.BadRequest));

        var ex = await Assert.ThrowsAnyAsync<Exception>(() => FakeHttp.CreateClient(handler).PutMetricDataAsync(new PutMetricDataRequest { Namespace = "N" }.With(Datum())));

        Assert.Contains("BadRequest", ex.Message);
        Assert.Contains("InvalidParameterValue", ex.Message);
    }

    // API reference sample "a single metric containing one dimension"
    [Fact]
    public async Task MatchesDocumentedSingleDimensionSample()
    {
        var sent = await SendAsync(new PutMetricDataRequest { Namespace = "TestNamespace" }.With(new MetricDatum {
            MetricName = "buffers",
            Unit = "Bytes",
            Value = 231434333,
            Dimensions = [Dim("InstanceType", "m1.small")]
        }));

        Assert.Equal([
            new("Action", "PutMetricData"),
            new("Namespace", "TestNamespace"),
            new("MetricData.member.1.MetricName", "buffers"),
            new("MetricData.member.1.Unit", "Bytes"),
            new("MetricData.member.1.Value", "231434333"),
            new("MetricData.member.1.Dimensions.member.1.Name", "InstanceType"),
            new("MetricData.member.1.Dimensions.member.1.Value", "m1.small"),
            new("Version", "2010-08-01")
        ], sent.Form);
    }

    // API reference sample "a single metric containing two dimensions"
    [Fact]
    public async Task MatchesDocumentedTwoDimensionSample()
    {
        var sent = await SendAsync(new PutMetricDataRequest { Namespace = "TestNamespace" }.With(new MetricDatum {
            MetricName = "buffers",
            Unit = "Bytes",
            Value = 231434333,
            Dimensions = [Dim("InstanceID", "i-aaba32d4"), Dim("InstanceType", "m1.small")]
        }));

        Assert.Equal([
            new("Action", "PutMetricData"),
            new("Namespace", "TestNamespace"),
            new("MetricData.member.1.MetricName", "buffers"),
            new("MetricData.member.1.Unit", "Bytes"),
            new("MetricData.member.1.Value", "231434333"),
            new("MetricData.member.1.Dimensions.member.1.Name", "InstanceID"),
            new("MetricData.member.1.Dimensions.member.1.Value", "i-aaba32d4"),
            new("MetricData.member.1.Dimensions.member.2.Name", "InstanceType"),
            new("MetricData.member.1.Dimensions.member.2.Value", "m1.small"),
            new("Version", "2010-08-01")
        ], sent.Form);
    }

    // User guide "Publish statistic sets": a CLI command, so only its values come from the docs
    [Fact]
    public async Task EncodesDocumentedStatisticSetSample()
    {
        var sent = await SendAsync(new PutMetricDataRequest { Namespace = "MyService" }.With(new MetricDatum {
            MetricName = "PageViewCount",
            StatisticValues = new StatisticSet { Sum = 11, Minimum = 2, Maximum = 5, SampleCount = 3 },
            Timestamp = new DateTime(2016, 10, 14, 12, 00, 00, DateTimeKind.Utc)
        }));

        Assert.Equal([
            new("Action", "PutMetricData"),
            new("Namespace", "MyService"),
            new("MetricData.member.1.MetricName", "PageViewCount"),
            new("MetricData.member.1.Timestamp", "2016-10-14T12:00:00Z"),
            new("MetricData.member.1.StatisticValues.Maximum", "5"),
            new("MetricData.member.1.StatisticValues.Minimum", "2"),
            new("MetricData.member.1.StatisticValues.SampleCount", "3"),
            new("MetricData.member.1.StatisticValues.Sum", "11"),
            new("Version", "2010-08-01")
        ], sent.Form);
    }

    // API reference sample "two metrics, each with two dimensions"
    [Fact]
    public async Task MatchesDocumentedSampleRequest()
    {
        var sent = await SendAsync(new PutMetricDataRequest { Namespace = "TestNamespace" }.With(
            new MetricDatum {
                MetricName = "buffers",
                Unit = "Bytes",
                Value = 231434333,
                Dimensions = [Dim("InstanceID", "i-aaba32d4"), Dim("InstanceType", "m1.small")]
            },
            new MetricDatum {
                MetricName = "latency",
                Unit = "Milliseconds",
                Value = 23,
                Dimensions = [Dim("InstanceID", "i-aaba32d4"), Dim("InstanceType", "m1.small")]
            }));

        Assert.Equal([
            new("Action", "PutMetricData"),
            new("Namespace", "TestNamespace"),
            new("MetricData.member.1.MetricName", "buffers"),
            new("MetricData.member.1.Unit", "Bytes"),
            new("MetricData.member.1.Value", "231434333"),
            new("MetricData.member.1.Dimensions.member.1.Name", "InstanceID"),
            new("MetricData.member.1.Dimensions.member.1.Value", "i-aaba32d4"),
            new("MetricData.member.1.Dimensions.member.2.Name", "InstanceType"),
            new("MetricData.member.1.Dimensions.member.2.Value", "m1.small"),
            new("MetricData.member.2.MetricName", "latency"),
            new("MetricData.member.2.Unit", "Milliseconds"),
            new("MetricData.member.2.Value", "23"),
            new("MetricData.member.2.Dimensions.member.1.Name", "InstanceID"),
            new("MetricData.member.2.Dimensions.member.1.Value", "i-aaba32d4"),
            new("MetricData.member.2.Dimensions.member.2.Name", "InstanceType"),
            new("MetricData.member.2.Dimensions.member.2.Value", "m1.small"),
            new("Version", "2010-08-01")
        ], sent.Form);
    }

    [Fact]
    public async Task DimensionKeysAreSeparatedFromMemberNumber()
    {
        var sent = await SendAsync(new PutMetricDataRequest { Namespace = "N" }.With(Datum("a", Dim("InstanceType", "m1.small"))));

        Assert.Equal("InstanceType", sent["MetricData.member.1.Dimensions.member.1.Name"]);
    }

    [Fact]
    public async Task SendsDimensionValue()
    {
        var sent = await SendAsync(new PutMetricDataRequest { Namespace = "N" }.With(Datum("a", Dim("InstanceType", "m1.small"))));

        Assert.Equal(["m1.small"], sent.ValuesWithKeySuffix("Dimensions.member.1.Value"));
    }

    [Fact]
    public async Task SendsEveryDimension()
    {
        var sent = await SendAsync(new PutMetricDataRequest { Namespace = "N" }.With(Datum("a", Dim("First", "1"), Dim("Second", "2"))));

        Assert.Equal(["First"], sent.ValuesWithKeySuffix("Dimensions.member.1.Name"));
        Assert.Equal(["Second"], sent.ValuesWithKeySuffix("Dimensions.member.2.Name"));
    }

    [Fact]
    public async Task DimensionsDoNotDropFollowingDatum()
    {
        var sent = await SendAsync(new PutMetricDataRequest { Namespace = "N" }.With(Datum("a", Dim("D", "1")), Datum("b")));

        Assert.Equal("b", sent["MetricData.member.2.MetricName"]);
    }

    [Fact]
    public async Task LaterDatumKeepsItsDimensions()
    {
        var sent = await SendAsync(new PutMetricDataRequest { Namespace = "N" }.With(Datum("a"), Datum("b", Dim("D", "1"))));

        Assert.Equal(["D"], sent.ValuesWithKeySuffix("Dimensions.member.1.Name"));
    }

    [Fact]
    public async Task SendsTimestamp()
    {
        var datum = Datum();

        datum.Timestamp = new DateTime(2024, 11, 06, 02, 16, 28, DateTimeKind.Utc);

        var sent = await SendAsync(new PutMetricDataRequest { Namespace = "N" }.With(datum));

        Assert.Equal("2024-11-06T02:16:28Z", sent["MetricData.member.1.Timestamp"]);
    }

    // Only meaningful where the local time zone isn't UTC; run with TZ=America/New_York to exercise it
    [Fact]
    public async Task ConvertsLocalTimestampToUtc()
    {
        var datum = Datum();

        datum.Timestamp = new DateTime(2024, 11, 06, 02, 16, 28, DateTimeKind.Local);

        var sent = await SendAsync(new PutMetricDataRequest { Namespace = "N" }.With(datum));

        Assert.Equal(datum.Timestamp.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture), sent["MetricData.member.1.Timestamp"]);
    }

    [Fact]
    public async Task SendsStatisticValuesInsteadOfValue()
    {
        var sent = await SendAsync(new PutMetricDataRequest { Namespace = "N" }.With(new MetricDatum {
            MetricName = "a",
            Unit = "Count",
            StatisticValues = new StatisticSet { Maximum = 30, Minimum = 1, SampleCount = 3, Sum = 36 }
        }));

        Assert.Equal("30", sent["MetricData.member.1.StatisticValues.Maximum"]);
        Assert.Equal("1", sent["MetricData.member.1.StatisticValues.Minimum"]);
        Assert.Equal("3", sent["MetricData.member.1.StatisticValues.SampleCount"]);
        Assert.Equal("36", sent["MetricData.member.1.StatisticValues.Sum"]);
        Assert.Null(sent["MetricData.member.1.Value"]);
    }

    [Fact]
    public async Task OmitsUnsetUnit()
    {
        var sent = await SendAsync(new PutMetricDataRequest { Namespace = "N" }.With(new MetricDatum { MetricName = "a", Value = 1 }));

        Assert.DoesNotContain(sent.Form, f => f.Key == "MetricData.member.1.Unit");
    }

    [Fact]
    public async Task FormatsValueWithInvariantCulture()
    {
        var original = CultureInfo.CurrentCulture;

        // Built by hand since the container may run in invariant globalization mode
        var commaDecimal = (CultureInfo)CultureInfo.InvariantCulture.Clone();
        commaDecimal.NumberFormat.NumberDecimalSeparator = ",";

        CultureInfo.CurrentCulture = commaDecimal;

        try
        {
            var datum = Datum();

            datum.Value = 1.5;

            var sent = await SendAsync(new PutMetricDataRequest { Namespace = "N" }.With(datum));

            Assert.Equal("1.5", sent["MetricData.member.1.Value"]);
        }
        finally
        {
            CultureInfo.CurrentCulture = original;
        }
    }
}

file static class PutMetricDataRequestExtensions
{
    public static PutMetricDataRequest With(this PutMetricDataRequest request, params MetricDatum[] data)
    {
        request.AddRange(data);

        return request;
    }
}
