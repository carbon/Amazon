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
        var q = "=?utf-8?Q?\"Jo=C3=A3o\" <x@x>?=";

        MailAddress.TryCreate(QuotedPrintable.Decode(q), out var r);

        Assert.Equal("João", r.DisplayName);
        Assert.Equal("x@x", r.Address);
    }
}
