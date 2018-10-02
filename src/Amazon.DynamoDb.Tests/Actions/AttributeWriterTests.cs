using System.Collections.Generic;
using System.IO;

using Carbon.Json;

using Xunit;

namespace Amazon.DynamoDb.Models.Tests
{
    public class AttributeWriterTests
    {
        [Fact]
        public void Json_test_1()
        {
            var sb = new StringWriter();

            var writer = new AttributeWriter(sb);

            var json = JsonObject.FromObject(new
            {
                a = 1,
                b = 1.1,
                c = "a",
                d = new[] { "a", "b", "c" },
                e = new List<string> { "a", "b", "c" },
                f = true,
                g = false
            });

            writer.WriteJsonObject(json);

            Assert.Equal(
                expected: @"{""a"":{""N"":""1""},""b"":{""N"":""1.1""},""c"":{""S"":""a""},""d"":{""L"":[{""S"":""a""},{""S"":""b""},{""S"":""c""}]},""e"":{""L"":[{""S"":""a""},{""S"":""b""},{""S"":""c""}]},""f"":{""BOOL"":true},""g"":{""BOOL"":false}}",
                actual: sb.ToString()
            );
        }

        [Fact]
        public void Json_test_7()
        {
            var sb = new StringWriter();

            var writer = new AttributeWriter(sb);

            var json = JsonObject.FromObject(new
            {
                a = new {
                    a = 1,
                    b = 1.1,
                    c = "a",
                    d = new[] { "a", "b", "c" },
                    e = new List<string> { "a", "b", "c" },
                    f = true,
                    g = false
                }
            });

            writer.WriteJsonObject(json);


            Assert.Equal(@"{""a"":{""M"":{""a"":{""N"":""1""},""b"":{""N"":""1.1""},""c"":{""S"":""a""},""d"":{""L"":[{""S"":""a""},{""S"":""b""},{""S"":""c""}]},""e"":{""L"":[{""S"":""a""},{""S"":""b""},{""S"":""c""}]},""f"":{""BOOL"":true},""g"":{""BOOL"":false}}}}",
                actual: sb.ToString());
        }

        [Fact]
        public void Json_test_15()
        {
            var sb = new StringWriter();

            var writer = new AttributeWriter(sb);

            var json = JsonObject.FromObject(new {
               a = new[] { 1, 2, 3 },
            });

            writer.WriteJsonObject(json);

            Assert.Equal(@"{""a"":{""L"":[{""N"":""1""},{""N"":""2""},{""N"":""3""}]}}",
                actual: sb.ToString());
        }

        [Fact]
        public void Json_test_5()
        {
            var sb = new StringWriter();

            var writer = new AttributeWriter(sb);

            var json = JsonObject.FromObject(new
            {
                a = new HashSet<string>(new[] { "a" })
            });

            writer.WriteJsonObject(json);

            Assert.Equal(
                expected: @"{""a"":{""SS"":[""a""]}}",
                actual: sb.ToString());


        }

        [Fact]
        public void Json_test_19()
        {
            var sb = new StringWriter();

            var writer = new AttributeWriter(sb);

            var value = new DbValue(new AttributeCollection {
                { "a", 1 },
                { "b", "boat" }
            });


            writer.WriteDbValue(value);

            Assert.Equal(@"{""M"":{""a"":{""N"":""1""},""b"":{""S"":""boat""}}}", sb.ToString());
        }

        [Fact]
        public void Json_test_2()
        {
            var sb = new StringWriter();

            var writer = new AttributeWriter(sb);

            var json = JsonObject.FromObject(new
            {
                a = new HashSet<string> { "a", "b", "c" },
            });

            writer.WriteJsonObject(json);

            Assert.Equal(
                expected: @"{""a"":{""SS"":[""a"",""b"",""c""]}}",
                actual: sb.ToString());
        }


        [Fact]
        public void Json_test_3()
        {
            var sb = new StringWriter();

            var writer = new AttributeWriter(sb);

            var json = JsonObject.FromObject(new
            {
                a = new HashSet<int> { 1, 2, 3 }
            });

            writer.WriteJsonObject(json);

            Assert.Equal(
                expected: @"{""a"":{""NS"":[""1"",""2"",""3""]}}",
                actual: sb.ToString());
        }
    }
}
