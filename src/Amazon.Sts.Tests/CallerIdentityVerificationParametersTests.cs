using System.Collections.Generic;
using System.Text.Json;
using Xunit;

namespace Amazon.Sts.Tests
{
    public class CallerIdentityVerificationParametersTests
    {
        [Fact]
        public void CanRoundtrip()
        {
            var a = new CallerIdentityVerificationParameters
            {
                Body = "body",
                Headers = new Dictionary<string, string>
                {
                    { "a", "1" },
                    { "b", "2" }
                },
                Url = "url"
            };

            Assert.Equal(@"{""url"":""url"",""headers"":{""a"":""1"",""b"":""2""},""body"":""body""}", JsonSerializer.Serialize(a));

            var b = JsonSerializer.Deserialize<CallerIdentityVerificationParameters>(JsonSerializer.Serialize(a));

            Assert.Equal("body", b.Body);
            Assert.Equal("url", b.Url);

            Assert.Equal("1", b.Headers["a"]);
            Assert.Equal("2", b.Headers["b"]);

            Assert.Equal(typeof(Dictionary<string, string>), b.Headers.GetType());
        }
    }
}
