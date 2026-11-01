using System.Net.Mail;

namespace Amazon.Ses.Tests;

public class QuotedPrintableTests
{
    [Fact]
    public void Decode()
    {
        Assert.Equal("Subject", QuotedPrintable.Decode("=?utf-8?Q?Subject?="));
        Assert.Equal("☻", QuotedPrintable.Decode("=?UTF-8?Q?=E2=98=BB?="));
        Assert.Equal("Як ти поживаєш?", QuotedPrintable.Decode("=?utf-8?Q?=D0=AF=D0=BA_=D1=82=D0=B8_=D0=BF=D0=BE=D0=B6=D0=B8=D0=B2=D0=B0=D1=94=D1=88=3F?="));
    }

    [Fact]
    public void Encode()
    {
        // 309ms per 1M

        Assert.Equal("=?utf-8?Q?=E2=98=BB?=", QuotedPrintable.Encode("☻"));
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("Subject", "Subject")]
    [InlineData("a b", "=?utf-8?Q?a_b?=")]
    [InlineData("a ", "=?utf-8?Q?a=20?=")]
    [InlineData("a=b", "=?utf-8?Q?a=3Db?=")]
    [InlineData("a?", "=?utf-8?Q?a=3F?=")]
    [InlineData("a\tb", "=?utf-8?Q?a=09b?=")]
    public void Encode_EscapesOnlyWhatIsRequired(string text, string expected)
    {
        Assert.Equal(expected, QuotedPrintable.Encode(text));
    }

    [Theory]
    [InlineData("☻")]
    [InlineData("Як ти поживаєш?")]
    public void EncodedTextRoundTrips(string text)
    {
        Assert.Equal(text, QuotedPrintable.Decode(QuotedPrintable.Encode(text)));
    }

    [Fact]
    public void Encode_LongText()
    {
        // Larger than the stack buffer, and grows the builder past its initial capacity
        var text = new string('☻', 200);

        var encoded = QuotedPrintable.Encode(text);

        Assert.Equal("=?utf-8?Q?".Length + (200 * 9) + "?=".Length, encoded.Length);
        Assert.StartsWith("=?utf-8?Q?=E2=98=BB", encoded);
        Assert.EndsWith("=E2=98=BB?=", encoded);
    }

    [Fact]
    public void Decode_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => QuotedPrintable.Decode(null));
    }

    [Theory]
    [InlineData(null, 0)]
    [InlineData("", 0)]
    [InlineData("abc", 3)]
    [InlineData("a b", 3)]
    [InlineData("a ", 4)]
    [InlineData("a=?", 7)]
    [InlineData("☻", 9)]
    public void GetEncodedCharCount(string value, int expected)
    {
        Assert.Equal(expected, QuotedPrintable.GetEncodedCharCount(value));
    }

    [Fact]
    public void GetEncodedCharCount_LongText()
    {
        Assert.Equal(200 * 9, QuotedPrintable.GetEncodedCharCount(new string('☻', 200)));
    }

    [Fact]
    public void DeodeUtf8Code()
    {
        var q = "=?UTF-8?Q?=E0=B8=99=E0=B8=A0=E0=B8=B1=E0=B8=AA=E0=B8=AA=E0=B8=A3?=";

        var r = QuotedPrintable.Decode(q);

        Assert.Equal("นภัสสร", r);
    }

    [Fact]
    public void DeodeUtf8Code_WithSpace()
    {
        var q = """=?utf-8?Q?"Jo=C3=A3o" <x@x>?=""";

        MailAddress.TryCreate(QuotedPrintable.Decode(q), out var r);

        Assert.Equal("João", r.DisplayName);
        Assert.Equal("x@x", r.Address);
    }
}
