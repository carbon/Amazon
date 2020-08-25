using System.Collections.Generic;
using System.Text.Json;

using Xunit;

namespace Amazon.Sts.Tests
{
    public class StsHelperTests
    {
        [Fact]
        public void A()
        {
            var request = new GetCallerIdentityRequest();

            var dic = ToParams(request);

            Assert.Single(dic);

            Assert.Equal("GetCallerIdentity", dic["Action"]);
        }

        [Fact]
        public void B()
        {
            var request = new GetFederationTokenRequest("name", durationSeconds: 30);

            var dic = ToParams(request);

            Assert.Equal(3, dic.Count);

            Assert.Equal("30", dic["DurationSeconds"]);
        }

        private static readonly JsonSerializerOptions jso = new () { IgnoreNullValues = true };

        public static Dictionary<string, string> ToParams<T>(T instance)
            where T: IStsRequest
        {
            using var doc = JsonDocument.Parse(JsonSerializer.SerializeToUtf8Bytes(instance, jso));

            var parameters = new Dictionary<string, string>();

            foreach (var member in doc.RootElement.EnumerateObject())
            {
                parameters.Add(member.Name, member.Value.ToString());
            }

            return parameters;
        }
    }
}
