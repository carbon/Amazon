using System.Net.Mail;

namespace Amazon.Ses.Tests;

public class SesHelperTests
{
    [Theory]
    [InlineData("user@test.com", true)]
    [InlineData("user@bücher.example", true)]
    [InlineData("üser@test.com", false)]
    public void IsSupported(string address, bool expected)
    {
        Assert.Equal(expected, SesHelper.IsSupported(new MailAddress(address)));
    }

    [Theory]
    [InlineData("User@Test.com", "user@test.com")]
    [InlineData("\"Name\" <User@Test.com>", "user@test.com")]
    [InlineData("""=?UTF-8?Q?"Jo=C3=A3o" <User@Test.com>?=""", "user@test.com")]
    public void DecodeMailAddress(string address, string expected)
    {
        Assert.Equal(expected, SesHelper.DecodeMailAddress(address));
    }

    [Fact]
    public void DecodeMailAddress_ThrowsWhenInvalid()
    {
        var ex = Assert.Throws<Exception>(() => SesHelper.DecodeMailAddress("invalid"));

        Assert.Equal("error parsing email - 'invalid'", ex.Message);
    }

    [Fact]
    public void EncodeMailAddress_WithoutDisplayName()
    {
        Assert.Equal("user@test.com", SesHelper.EncodeMailAddress(new MailAddress("user@test.com")));
    }

    [Theory]
    [InlineData("Name", "\"Name\" <user@test.com>")]
    [InlineData("First Last", "\"=?utf-8?Q?First_Last?=\" <user@test.com>")]
    [InlineData("☻", "\"=?utf-8?Q?=E2=98=BB?=\" <user@test.com>")]
    public void EncodeMailAddress_WithDisplayName(string displayName, string expected)
    {
        Assert.Equal(expected, SesHelper.EncodeMailAddress(new MailAddress("user@test.com", displayName)));
    }

    [Fact]
    public void EncodeMailAddress_OmitsLongDisplayName()
    {
        var address = new MailAddress("user@test.com", new string('a', 128));

        Assert.Equal("<user@test.com>", SesHelper.EncodeMailAddress(address));
    }

    [Fact]
    public void EncodeMailAddress_ConvertsHostToPunycode()
    {
        Assert.Equal("user@xn--bcher-kva.example", SesHelper.EncodeMailAddress(new MailAddress("user@bücher.example")));
    }
}
