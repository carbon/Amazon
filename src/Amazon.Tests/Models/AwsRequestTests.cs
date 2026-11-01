namespace Amazon.Tests;

public class AwsRequestTests
{
    [Fact]
    public void AddsParameters()
    {
        var request = new AwsRequest {
            { "a", "1" },
            { "b", 2 },
            new KeyValuePair<string, string>("c", "3")
        };

        request.AddRange([new("d", "4"), new("e", "5")]);

        Assert.Equal(["a=1", "b=2", "c=3", "d=4", "e=5"], request.Select(static p => $"{p.Key}={p.Value}"));
        Assert.Equal(5, request.Parameters.Count);
    }

    [Fact]
    public void CanStartWithParameters()
    {
        var request = new AwsRequest([new("a", "1")]);

        Assert.Equal("a", Assert.Single(request.Parameters).Key);
    }

    [Fact]
    public void EnumeratesWithoutGenerics()
    {
        int count = 0;

        foreach (var _ in (System.Collections.IEnumerable)new AwsRequest { { "a", "1" } })
        {
            count++;
        }

        Assert.Equal(1, count);
    }
}
