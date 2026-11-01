using System.Globalization;

using System.Text;

namespace Amazon.Helpers.Tests;

public class DictionaryExtensionsTests
{
    [Fact]
    public void EmptyListsReturnEmptyStrings()
    {
        Assert.Equal("", new List<KeyValuePair<string, string>>().ToPostData());
        Assert.Equal("", new List<KeyValuePair<string, string>>().ToQueryString());
        Assert.Equal("", ((List<KeyValuePair<string, string>>)null).ToPostData());
        Assert.Equal("", ((List<KeyValuePair<string, string>>)null).ToQueryString());
    }

    [Fact]
    public void ToPostData_JoinsAndEncodesValues()
    {
        List<KeyValuePair<string, string>> items = [new("a", "1 2"), new("b", "x&y")];

        Assert.Equal("a=1%202&b=x%26y", items.ToPostData());
    }

    [Fact]
    public void ToQueryString_AddsPrefix()
    {
        List<KeyValuePair<string, string>> items = [new("a", "1"), new("b", "2")];

        Assert.Equal("?a=1&b=2", items.ToQueryString());
    }

    [Fact]
    public void SkipsEmptyKeys()
    {
        List<KeyValuePair<string, string>> items = [new("", "skipped"), new("a", "1"), new(null, "skipped"), new("b", "2")];

        Assert.Equal("a=1&b=2", items.ToPostData());
        Assert.Equal("?a=1&b=2", items.ToQueryString());
    }

    [Fact]
    public void SupportsLongValues()
    {
        var value = new string('a', 500);

        List<KeyValuePair<string, string>> items = [new("a", value)];

        Assert.Equal($"a={value}", items.ToPostData());
    }
}

public class StringListPoolTests
{
    [Fact]
    public void ReusesReturnedLists()
    {
        var pool = new StringListPool(1);

        var list = pool.Rent();

        list.Add("a");

        pool.Return(list);

        Assert.Empty(list);
        Assert.Same(list, pool.Rent());
    }

    [Fact]
    public void CreatesListWhenEmpty()
    {
        var pool = new StringListPool(1);

        Assert.NotSame(pool.Rent(), pool.Rent());
    }

    [Fact]
    public void DropsListsWhenFull()
    {
        var pool = new StringListPool(1);

        var a = pool.Rent();
        var b = pool.Rent();

        pool.Return(a);
        pool.Return(b);

        Assert.Same(a, pool.Rent());
        Assert.NotSame(b, pool.Rent());
    }

    [Fact]
    public void DoesNotKeepLargeLists()
    {
        var pool = new StringListPool(1);

        var list = pool.Rent();

        for (int i = 0; i < 33; i++)
        {
            list.Add("a");
        }

        pool.Return(list);

        Assert.NotSame(list, pool.Rent());
    }
}

public class ValueStringBuilderTests
{
    [Fact]
    public void GrowsPastInitialBuffer()
    {
        var sb = new ValueStringBuilder(stackalloc char[2]);

        sb.Append('a');
        sb.Append('b');
        sb.Append('c');
        sb.Append("de");
        sb.Append("fgh".AsSpan());
        sb.Append('x', 3);

        Assert.Equal("abcdefghxxx", sb.ToString());
    }

    [Fact]
    public void GrowsFromRentedBuffer()
    {
        var sb = new ValueStringBuilder(16);

        sb.Append(new string('a', 100));
        sb.Append(new string('b', 100));

        Assert.Equal(200, sb.Length);

        string result = sb.ToString();

        Assert.Equal(new string('a', 100) + new string('b', 100), result);
    }

    [Fact]
    public void AppendIgnoresNull()
    {
        var sb = new ValueStringBuilder(8);

        sb.Append((string)null);

        Assert.Equal("", sb.ToString());
    }

    [Fact]
    public void AppendJoin()
    {
        var sb = new ValueStringBuilder(8);

        sb.AppendJoin(';', ["a", "b", "c"]);

        Assert.Equal("a;b;c", sb.ToString());
    }

    [Fact]
    public void AppendSpan_ReturnsWritableRegion()
    {
        var sb = new ValueStringBuilder(stackalloc char[1]);

        "abc".AsSpan().CopyTo(sb.AppendSpan(3));

        Assert.Equal("abc", sb.ToString());
    }

    [Fact]
    public void AppendSpanFormattable_FallsBackWhenBufferIsFull()
    {
        var sb = new ValueStringBuilder(stackalloc char[1]);

        sb.AppendSpanFormattable(12345, provider: CultureInfo.InvariantCulture);

        Assert.Equal("12345", sb.ToString());
    }

    [Fact]
    public void ExposesContents()
    {
        var sb = new ValueStringBuilder(8);

        sb.Append("abcd");

        Assert.Equal('b', sb[1]);
        Assert.Equal("abcd", sb.AsSpan().ToString());
        Assert.Equal("cd", sb.AsSpan(2).ToString());
        Assert.Equal("bc", sb.AsSpan(1, 2).ToString());
        Assert.True(sb.Capacity >= 8);

        sb.Length = 2;

        Assert.Equal("ab", sb.ToString());
    }
}
