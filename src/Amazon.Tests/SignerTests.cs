using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;

using Carbon.Data.Encodings;

namespace Amazon.Security.Tests;

public class SignerTests
{
    [Fact]
    public void Encode()
    {
        Assert.Equal("abc%3D34", UrlEncoder.Default.Encode("abc=34"));
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/fruit/apple")]
    [InlineData("/bananas")]
    [InlineData("/-._~0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz")]
    public void UnreservedCanonicalUris(string text)
    {
        Assert.Equal(text, SignerV4.CanonicalizeUri(text));
    }

    [Fact]
    public void UnicodeCanicalUri()
    {
        Assert.Equal("/%E1%88%B4", SignerV4.CanonicalizeUri("ሴ"));
        Assert.Equal("/frame%3A11", SignerV4.CanonicalizeUri("/frame:11"));
        Assert.Equal("/frame%3A11%4030s", SignerV4.CanonicalizeUri("/frame:11@30s"));
    }

    [Fact]
    public void DoubleSlashesAreNormalized()
    {
        Assert.Equal("/a/b/c", SignerV4.CanonicalizeUri("//a//b///c"));
    }

    [Fact]
    public void CanonicizeQueryStrings()
    {
        string a1000 = "".PadLeft(1000, 'a');

        Assert.Equal("a=", SignerV4.CanonicalizeQueryString(new Uri("http://a.com/?a")));
        Assert.Equal("a=1", SignerV4.CanonicalizeQueryString(new Uri("http://a.com/?a=1")));
        Assert.Equal("a=1&b=2&c=3", SignerV4.CanonicalizeQueryString(new Uri("http://a.com/?c=3&b=2&a=1")));
        Assert.Equal("a=" + a1000, SignerV4.CanonicalizeQueryString(new Uri("http://a.com/?a=" + a1000)));
    }

    [Fact]
    public void SigningKey()
    {
        var scope = new CredentialScope(
            date    : new DateTime(2012, 02, 15),
            region  : AwsRegion.USEast1,
            service : AwsService.Iam
        );

        byte[] key = SignerV4.ComputeSigningKey("wJalrXUtnFEMI/K7MDENG+bPxRfiCYEXAMPLEKEY", scope);

        Assert.Equal("f4780e2d9f65fa895f9c67b32ce1baf0b0d8a43505a000a1a9e090d414db404d", HexString.FromBytes(key));
    }

    [Fact]
    public void SigningKey2()
    {
        var scope = new CredentialScope(
            date    : new DateTime(2022, 02, 15),
            region  : AwsRegion.USWest1,
            service : AwsService.Iam
        );

        byte[] key = SignerV4.ComputeSigningKey("wJalrXUtnFEMI/K7MDENG+bPxRfiCYEXAMPLE123", scope);

        Assert.Equal("06e1b996efb4c3080f04cfd3fb6e8151b1b89ec05a3d0dc7886ecd08f7b2d240", HexString.FromBytes(key));
    }

    [Fact]
    public void Sign1()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "http://dynamodb.us-east-1.amazonaws.com/");

        request.Headers.Date = new DateTimeOffset(2012, 02, 17, 18, 31, 22, TimeSpan.Zero);
        request.Headers.Host = "dynamodb.us-east-1.amazonaws.com";

        request.Headers.Add("x-amz-security-token", "123");
        request.Headers.Add("x-amz-target", "DynamoDB_20111205.ListTables");
        request.Headers.Add("x-amz-date", "2012-02-17");

        var cred = new AwsCredential("", "wJalrXUtnFEMI/K7MDENG+bPxRfiCYEXAMPLEKEY");

        SignerV4.Sign(cred, dynamoScope, request);

        var auth = request.Headers.GetValues("Authorization").First();

        Assert.Equal("AWS4-HMAC-SHA256 Credential=/20120215/us-east-1/dynamodb/aws4_request,SignedHeaders=host;x-amz-date;x-amz-security-token;x-amz-target,Signature=204ce0e8f0d2aec6fb328b4b34df0a8bc5363c11f01df2a489e788b3703be4e5", auth);
    }

    [Fact]
    public void CanonlizeRequest()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "http://dynamodb.us-east-1.amazonaws.com/")
        {
            Headers = {
                { "x-amz-date", "2012-02-17" },
                { "x-amz-content-sha256", "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b785" }
            }
        };

        request.Headers.Date = new DateTimeOffset(2012, 02, 17, 18, 31, 22, TimeSpan.Zero);
        request.Headers.Host = "s3.us-east-1.amazonaws.com";

        Assert.Equal(@"host:s3.us-east-1.amazonaws.com
x-amz-content-sha256:e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b785
x-amz-date:2012-02-17".Replace("\r", ""), SignerV4.CanonicalizeHeaders(request, out var signedHeaderNames));

        Assert.Equal("host;x-amz-content-sha256;x-amz-date", string.Join(';', signedHeaderNames));


        Assert.Equal(@"POST
/

host:s3.us-east-1.amazonaws.com
x-amz-content-sha256:e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b785
x-amz-date:2012-02-17

host;x-amz-content-sha256;x-amz-date
e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b785".Replace("\r", ""), SignerV4.GetCanonicalRequest(request));
    }


    [Fact]
    public void CanonlizeRequest_MixedCase_OutOfOrder()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "http://dynamodb.us-east-1.amazonaws.com/") {
            Headers = {
                { "x-amz-date", "2012-02-17" },
                { "X-AMZ-A", "out-of-order"},
                { "x-amz-content-sha256", "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b785" }
            }
        };

        request.Headers.Date = new DateTimeOffset(2012, 02, 17, 18, 31, 22, TimeSpan.Zero);
        request.Headers.Host = "s3.us-east-1.amazonaws.com";

        Assert.Equal(@"host:s3.us-east-1.amazonaws.com
x-amz-a:out-of-order
x-amz-content-sha256:e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b785
x-amz-date:2012-02-17".Replace("\r", ""), SignerV4.CanonicalizeHeaders(request, out var signedHeaderNames));

        Assert.Equal("host;x-amz-a;x-amz-content-sha256;x-amz-date", string.Join(';', signedHeaderNames));


        Assert.Equal(@"POST
/

host:s3.us-east-1.amazonaws.com
x-amz-a:out-of-order
x-amz-content-sha256:e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b785
x-amz-date:2012-02-17

host;x-amz-a;x-amz-content-sha256;x-amz-date
e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b785".Replace("\r", ""), SignerV4.GetCanonicalRequest(request));
    }

    [Fact]
    public void CanonlizeRequestUnicode()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "http://example.amazonaws.com/ሴ") {
            Headers = {
                { "x-amz-date", "20150830T123600Z" },
            }
        };

        request.Headers.Date = new DateTimeOffset(2015, 08, 30, 18, 31, 22, TimeSpan.Zero);
        request.Headers.Host = "example.amazonaws.com";

        Assert.Equal(@"GET
/%E1%88%B4

host:example.amazonaws.com
x-amz-date:20150830T123600Z

host;x-amz-date
e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855

".Trim().Replace("\r", ""), SignerV4.GetCanonicalRequest(request));

        var scope = new CredentialScope(new DateTime(2015, 08, 30), AwsRegion.USEast1, (AwsService)("service"));

        var cred = new AwsCredential("AKIDEXAMPLE", "wJalrXUtnFEMI/K7MDENG+bPxRfiCYEXAMPLEKEY");

        SignerV4.Sign(cred, scope, request);

        var auth = request.Headers.GetValues("Authorization").First();

        Assert.Equal("AWS4-HMAC-SHA256 Credential=AKIDEXAMPLE/20150830/us-east-1/service/aws4_request,SignedHeaders=host;x-amz-date,Signature=8318018e0b0f223aa2bbf98705b62bb787dc9c0e678f255a891fd03141be5d85", auth);
    }

    [Fact]
    public void CanonlizeRequestUnsafeUrl()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "http://s3.us-east-1.amazonaws.com/frame:1") {
            Headers = {
                { "x-amz-date", "2012-02-17" },
                { "x-amz-content-sha256", "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b785" }
            }
        };

        request.Headers.Date = new DateTimeOffset(2012, 02, 17, 18, 31, 22, TimeSpan.Zero);
        request.Headers.Host = "s3.us-east-1.amazonaws.com";

        Assert.Equal(@"POST
/frame%3A1

host:s3.us-east-1.amazonaws.com
x-amz-content-sha256:e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b785
x-amz-date:2012-02-17

host;x-amz-content-sha256;x-amz-date
e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b785".Replace("\r", ""), SignerV4.GetCanonicalRequest(request));
    }


    [Fact]
    public void DoubleEscape()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "http://s3.us-east-1.amazonaws.com/frame%3A1")
        {
            Headers = {
                { "x-amz-date", "2012-02-17" },
                { "x-amz-content-sha256", "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b785" }
            }
        };

        request.Headers.Date = new DateTimeOffset(2012, 02, 17, 18, 31, 22, TimeSpan.Zero);
        request.Headers.Host = "s3.us-east-1.amazonaws.com";

        Assert.Equal(@"POST
/frame%3A1

host:s3.us-east-1.amazonaws.com
x-amz-content-sha256:e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b785
x-amz-date:2012-02-17

host;x-amz-content-sha256;x-amz-date
e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b785".Replace("\r", ""), SignerV4.GetCanonicalRequest(request));
    }

    [Fact]
    public void CanonlizeRequest_s3()
    {
        var request = new HttpRequestMessage(HttpMethod.Put, "http://s3.amazonaws.com/fruits/bananas") {
            Headers = {
                { "x-amz-date", "2012-02-17" },
                { "x-amz-content-sha256", "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b785" }
            }
        };

        request.Headers.Date = new DateTimeOffset(2012, 02, 17, 18, 31, 22, TimeSpan.Zero);
        request.Headers.Host = "s3.us-east-1.amazonaws.com";

        Assert.Equal(@"host:s3.us-east-1.amazonaws.com
x-amz-content-sha256:e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b785
x-amz-date:2012-02-17".Replace("\r", ""), SignerV4.CanonicalizeHeaders(request, out var signedHeaderNames));

        Assert.Equal("host;x-amz-content-sha256;x-amz-date", string.Join(';', signedHeaderNames));

        Assert.Equal(@"

PUT
/fruits/bananas

host:s3.us-east-1.amazonaws.com
x-amz-content-sha256:e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b785
x-amz-date:2012-02-17

host;x-amz-content-sha256;x-amz-date
e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b785


".Trim().Replace("\r", ""), SignerV4.GetCanonicalRequest(request));
    }

    [Fact]
    public void CanonlizeRequestWithContentMD5()
    {
        string textContent = "hello";

        var request = new HttpRequestMessage(HttpMethod.Post, "http://s3.amazonaws.com/") {
            Content = new StringContent(textContent, Encoding.UTF8) {
                Headers = {
                    ContentMD5 = MD5.HashData(Encoding.UTF8.GetBytes(textContent))
                }
            }
        };

        request.Headers.Date = new DateTimeOffset(2012, 02, 17, 18, 31, 22, TimeSpan.Zero);
        request.Headers.Host = "s3.amazonaws.com";

        Assert.Equal(@"content-md5:XUFAKrxLKna5cZ2REBfFkg==
date:Fri, 17 Feb 2012 18:31:22 GMT
host:s3.amazonaws.com".Replace("\r", ""), SignerV4.CanonicalizeHeaders(request, out var signedHeaderNames));

        Assert.Equal("content-md5;date;host", string.Join(';', signedHeaderNames));
    }

    [Fact]
    public void Presign1()
    {
        var key = new AwsCredential("carbon", "wJalrXUtnFEMI/K7MDENG+bPxRfiCYEXAMPLEKEY");

        var date = new DateTime(2016, 01, 01);

        var scope = new CredentialScope(date, AwsRegion.USEast1, AwsService.RdsDb);

        var request = new HttpRequestMessage(HttpMethod.Get, "https://carbon.db:3010?Action=connect&DBUser=carbon");

        SignerV4.Presign(key, scope, date, TimeSpan.FromMinutes(15), request);

        var signedUrl = request.RequestUri.ToString();

        Assert.Equal("https://carbon.db:3010/?Action=connect&DBUser=carbon&X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=carbon%2F20160101%2Fus-east-1%2Frds-db%2Faws4_request&X-Amz-Date=20160101T000000Z&X-Amz-Expires=900&X-Amz-SignedHeaders=host&X-Amz-Signature=7c7f54b5f2ae7de227c96b59680f1a443562785addb8c62c0f674b052c3ebfae", signedUrl.ToString());
    }

    [Fact]
    public void Presign1_fast()
    {
        var key = new AwsCredential("carbon", "wJalrXUtnFEMI/K7MDENG+bPxRfiCYEXAMPLEKEY");

        var date = new DateTime(2016, 01, 01);

        var scope = new CredentialScope(date, AwsRegion.USEast1, AwsService.RdsDb);

        string signedUrl = SignerV4.GetPresignedUrl(key, scope, date, TimeSpan.FromMinutes(15), HttpMethod.Get, new Uri("https://carbon.db:3010?Action=connect&DBUser=carbon"));

        Assert.Equal("https://carbon.db:3010/?Action=connect&DBUser=carbon&X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=carbon%2F20160101%2Fus-east-1%2Frds-db%2Faws4_request&X-Amz-Date=20160101T000000Z&X-Amz-Expires=900&X-Amz-SignedHeaders=host&X-Amz-Signature=7c7f54b5f2ae7de227c96b59680f1a443562785addb8c62c0f674b052c3ebfae", signedUrl.ToString());
    }

    [Fact]
    public void PresignWithCustomPort()
    {
        var key = new AwsCredential("carbon", "wJalrXUtnFEMI/K7MDENG+bPxRfiCYEXAMPLEKEY");

        var date = new DateTime(2016, 01, 01);

        var scope = new CredentialScope(date, AwsRegion.USEast1, AwsService.RdsDb);

        var request = new HttpRequestMessage(HttpMethod.Get,
            "https://carbon.db:3036?Action=connect&DBUser=carbon"
        );

        SignerV4.Presign(key, scope, date, TimeSpan.FromMinutes(15), request);

        var signedUrl = request.RequestUri.ToString();

        Assert.Equal("https://carbon.db:3036/?Action=connect&DBUser=carbon&X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=carbon%2F20160101%2Fus-east-1%2Frds-db%2Faws4_request&X-Amz-Date=20160101T000000Z&X-Amz-Expires=900&X-Amz-SignedHeaders=host&X-Amz-Signature=bb05008e5d99cfcd759e45c8bc3442a98e144d630602b5556abc42105615e139", signedUrl.ToString());
    }

    [Fact]
    public void SignWithContentSha256()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "http://dynamodb.us-east-1.amazonaws.com/")
        {
            Headers = {
                { "x-amz-security-token", "123" },
                { "x-amz-target", "DynamoDB_20111205.ListTables" },
                { "x-amz-date", "2012-02-17" },
                { "x-amz-content-sha256", "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b785" }
            }
        };

        request.Headers.Date = new DateTimeOffset(2012, 02, 17, 18, 31, 22, TimeSpan.Zero);
        request.Headers.Host = "s3.us-east-1.amazonaws.com";

        var cred = new AwsCredential("carbon", "wJalrXUtnFEMI/K7MDENG+bPxRfiCYEXAMPLEKEY");

        SignerV4.Sign(cred, dynamoScope, request);

        var auth = request.Headers.GetValues("Authorization").First();


        Assert.Equal(@"AWS4-HMAC-SHA256
2012-02-17
20120215/us-east-1/dynamodb/aws4_request
70fdf1d6922246b48d56d3e0c1f7cc0d5dfc79acf421cf066e2033ba0025af40".Replace(Environment.NewLine, "\n"), SignerV4.GetStringToSign(dynamoScope, request));

        Assert.Equal("AWS4-HMAC-SHA256 Credential=carbon/20120215/us-east-1/dynamodb/aws4_request,SignedHeaders=host;x-amz-content-sha256;x-amz-date;x-amz-security-token;x-amz-target,Signature=26b5130264847f3848c1649ebf97feebbbede1f23b0ac2b55e539d2c50d25594", auth);
    }

    [Fact]
    public void SignWithQueryString()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "http://dynamodb.us-east-1.amazonaws.com/?a=1");

        request.Headers.Date = new DateTimeOffset(2012, 02, 17, 18, 31, 22, TimeSpan.Zero);
        request.Headers.Host = "dynamodb.us-east-1.amazonaws.com";

        request.Headers.Add("x-amz-security-token", "123");
        request.Headers.Add("x-amz-target", "DynamoDB_20111205.ListTables");
        request.Headers.Add("x-amz-date", "2012-02-17");

        var cred = new AwsCredential("", "wJalrXUtnFEMI/K7MDENG+bPxRfiCYEXAMPLEKEY");

        SignerV4.Sign(cred, dynamoScope, request);

        var auth = request.Headers.GetValues("Authorization").First();

        Assert.Equal("AWS4-HMAC-SHA256 Credential=/20120215/us-east-1/dynamodb/aws4_request,SignedHeaders=host;x-amz-date;x-amz-security-token;x-amz-target,Signature=bd37d33dc88c8541d87532d129a2e12990f04aa346d6963cdb24361106b87dfa", auth);
    }

    [Fact]
    public void SignWithContent()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "http://dynamodb.us-east-1.amazonaws.com/?a=1")
        {
            Content = new StringContent("HELLO", Encoding.UTF8, "application/text")
        };

        request.Headers.Date = new DateTimeOffset(2012, 02, 17, 18, 31, 22, TimeSpan.Zero);
        request.Headers.Host = "dynamodb.us-east-1.amazonaws.com";

        request.Headers.Add("x-amz-security-token", "123");
        request.Headers.Add("x-amz-target", "DynamoDB_20111205.ListTables");
        request.Headers.Add("x-amz-date", "2012-02-17");

        var cred = new AwsCredential("", "wJalrXUtnFEMI/K7MDENG+bPxRfiCYEXAMPLEKEY");

        SignerV4.Sign(cred, dynamoScope, request);

        var auth = request.Headers.GetValues("Authorization").First();

        Assert.Equal("AWS4-HMAC-SHA256 Credential=/20120215/us-east-1/dynamodb/aws4_request,SignedHeaders=host;x-amz-date;x-amz-security-token;x-amz-target,Signature=d3b5f4611dbfe8ba5577df24f9f39ffe25a4133ec12cf088391747119ee4c30a", auth);
    }

    private static readonly CredentialScope dynamoScope = new(
        date: new DateTime(2012, 02, 15),
        region: AwsRegion.USEast1,
        service: AwsService.DynamoDb
    );

    /*
    [Fact]
    public void HashPayload()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "http://dynamodb.us-east-1.amazonaws.com/") {
            Content = new StringContent("Action=ListGroupsForUser&UserName=Test&Version=2010-05-08")
        };

        Assert.Equal("14a1b0cf5748461c63d3a5fee5e42ed623422b7b4fa62a58a57258f1a195cff8", SignerV4.GetPayloadHash(request));
    }
    */
}