using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Carbon.Data.Sequences;
using Carbon.Json;

using Xunit;

namespace Amazon.DynamoDb.Models.Tests
{
    public class DbValueTests
	{
		[Fact]
		public void EnumTest()
		{
			var a = ABCEnum.A;

			var enumType = Type.GetTypeCode(a.GetType());

			Assert.Equal(TypeCode.Int32, enumType);

			Assert.Equal(1, new DbValue(a).Value);
		}

		[Fact]
		public void DbValueTest1()
		{
			Assert.Equal("abc", new DbValue("abc").ToString());
			Assert.Equal("123", new DbValue(123).ToString());
			Assert.Equal(123,   new DbValue(123).ToInt());
		}

		[Fact]
		public void BinaryTests()
		{
			var value = DbValue.FromJson(JsonObject.Parse(@"{""B"":""dmFsdWU=""}"));

			Assert.Equal(DbValueType.B, value.Kind);
			Assert.Equal("dmFsdWU=", Convert.ToBase64String(value.ToBinary()));

			Assert.Equal(@"{""B"":""dmFsdWU=""}", value.ToJson().ToString(pretty: false));
		}

		[Fact]
		public void DbValueTypes()
		{
			Assert.Equal(DbValueType.B, new DbValue(Encoding.UTF8.GetBytes("abc")).Kind);
			Assert.Equal(DbValueType.S, new DbValue("hellow").Kind);
			Assert.Equal(DbValueType.N, new DbValue((short)123).Kind);
			Assert.Equal(DbValueType.N, new DbValue(123).Kind);
			Assert.Equal(DbValueType.N, new DbValue(123L).Kind);
			Assert.Equal(DbValueType.N, new DbValue(123d).Kind);
			Assert.Equal(DbValueType.N, new DbValue(123f).Kind);
			Assert.Equal(DbValueType.N, new DbValue(123m).Kind);

			Assert.Equal(DbValueType.N, new DbValue((object)(short)123).Kind);
			Assert.Equal(DbValueType.N, new DbValue((object)123).Kind);
			Assert.Equal(DbValueType.N, new DbValue((object)123L).Kind);
			Assert.Equal(DbValueType.N, new DbValue((object)123d).Kind);
			Assert.Equal(DbValueType.N, new DbValue((object)123f).Kind);
			Assert.Equal(DbValueType.N, new DbValue((object)123m).Kind);

			Assert.Equal(DbValueType.SS, new DbValue(new[] { "a", "b", "c" }).Kind);
			Assert.Equal(DbValueType.NS, new DbValue(new[] { 1, 2, 3 }).Kind);
			Assert.Equal(DbValueType.NS, new DbValue(new[] { 1L, 2L, 3L }).Kind);
			Assert.Equal(DbValueType.NS, new DbValue(new[] { 1f, 2f, 3f }).Kind);
			Assert.Equal(DbValueType.NS, new DbValue(new[] { 1d, 2d, 3d }).Kind);
		}

		[Fact]
		public void SetConversions()
		{
			Assert.Equal("1,2,3", string.Join(',', new DbValue(new[] { 1, 2, 3 }).ToSet<Int16>()));
			Assert.Equal("1,2,3", string.Join(',', new DbValue(new[] { 1, 2, 3 }).ToSet<Int32>()));
			Assert.Equal("1,2,3", string.Join(',', new DbValue(new[] { 1, 2, 3 }).ToSet<Int64>()));
			Assert.Equal("1,2,3", string.Join(',', new DbValue(new[] { 1, 2, 3 }).ToSet<float>()));
			Assert.Equal("1,2,3", string.Join(',', new DbValue(new[] { 1, 2, 3 }).ToSet<double>()));
			Assert.Equal("a,b,c", string.Join(',', new DbValue(new[] { "a", "b", "c" }).ToStringSet()));
		}

		[Fact]
		public void ArrayConversions()
		{
			Assert.Equal("1,2,3", string.Join(',', new DbValue(new[] { 1, 2, 3 }).ToArray<Int16>()));
			Assert.Equal("1,2,3", string.Join(',', new DbValue(new[] { 1, 2, 3 }).ToArray<Int32>()));
			Assert.Equal("1,2,3", string.Join(',', new DbValue(new[] { 1, 2, 3 }).ToArray<Int64>()));
			Assert.Equal("1,2,3", string.Join(',', new DbValue(new[] { 1, 2, 3 }).ToArray<float>()));
			Assert.Equal("1,2,3", string.Join(',', new DbValue(new[] { 1, 2, 3 }).ToArray<double>()));
		}

		[Fact]
		public void DbList()
		{
			var value = DbValue.FromJson(JsonObject.Parse(@"{ ""L"": [ { ""N"": ""1"" }, { ""N"":""2"" } ] }"));

			Assert.Equal(DbValueType.L, value.Kind);

			Assert.Equal(new[] { "1", "2" }, value.ToArray<string>());

			Assert.Equal(new[] { 1, 2 }, value.ToArray<int>());
			Assert.Equal(new[] { 1L, 2L }, value.ToArray<long>());
		}

        [Fact]
        public void DbMap()
        {
            var value = new DbValue(new AttributeCollection {
                { "a", 1 },
                { "b", "boat" }
            });

         
            Assert.Equal(@"{
  ""M"": {
    ""a"": {
      ""N"": ""1""
    },
    ""b"": {
      ""S"": ""boat""
    }
  }
}", value.ToJson().ToString());
        }

        [Fact]
        public void DbMap5()
        {
            var ips = new List<IPAddress> {
                IPAddress.Parse("192.168.1.1"),
                IPAddress.Parse("192.168.1.2")
            };

            var value = new DbValue(AttributeCollection.FromObject(new Machine { 
                Id = 1,
                Ips = ips
            }));

            Assert.Equal(@"{
  ""M"": {
    ""id"": {
      ""N"": ""1""
    },
    ""ips"": {
      ""L"": [
        {
          ""B"": ""wKgBAQ==""
        },
        {
          ""B"": ""wKgBAg==""
        }
      ]
    }
  }
}", value.ToJson().ToString());
        }


     
        [Fact]
        public void DbMap2()
        {
            var value = AttributeCollection.FromObject(new Hi { A = 
                new Nested  {
                    A = 1,
                    B = "boat"
                }});


            Assert.Equal(@"{
  ""a"": {
    ""M"": {
      ""a"": {
        ""N"": ""1""
      },
      ""b"": {
        ""S"": ""boat""
      }
    }
  }
}", value.ToJson().ToString());

            var a = AttributeCollection.FromJson(value.ToJson()).As<Hi>();

            Assert.Equal(1, a.A.A);
            Assert.Equal("boat", a.A.B);

        }

        [Fact]
        public void UidTest()
        {
            var id = Uid.Deserialize(Convert.FromBase64String("ZKWQKAIkHUiA4MGBMfFiGg=="));

            var value = AttributeCollection.FromObject(new Entity {
                Id = id
            });



            Assert.Equal(@"{
  ""id"": {
    ""B"": ""ZKWQKAIkHUiA4MGBMfFiGg==""
  }
}", value.ToJson().ToString());

            var a = AttributeCollection.FromJson(value.ToJson()).As<Entity>();

            Assert.Equal(id, a.Id);

        }

        [Fact]
        public void DbMap3()
        {
            var meta = new Meta {
                Name = "faces",
                Annotations = new[] {
                    new Annotation {
                        Id = 1,
                        Confidence = 1f,
                        Description = "dog",
                        Position = new Position { X = 1, Y = 2, Z = 3 }
                    }
                }
            };

            var value = AttributeCollection.FromObject(meta);

            Assert.Equal(@"{
  ""name"": {
    ""S"": ""faces""
  },
  ""annotations"": {
    ""L"": [
      {
        ""M"": {
          ""id"": {
            ""N"": ""1""
          },
          ""description"": {
            ""S"": ""dog""
          },
          ""position"": {
            ""M"": {
              ""x"": {
                ""N"": ""1""
              },
              ""y"": {
                ""N"": ""2""
              },
              ""z"": {
                ""N"": ""3""
              }
            }
          },
          ""score"": {
            ""N"": ""0""
          },
          ""confidence"": {
            ""N"": ""1""
          },
          ""topicality"": {
            ""N"": ""0""
          }
        }
      }
    ]
  }
}", value.ToJson().ToString());



            var a = AttributeCollection.FromJson(value.ToJson()).As<Meta>();

            Assert.Equal("dog", a.Annotations[0].Description);
            Assert.Equal(1f, a.Annotations[0].Position.X);

        }


        [Fact]
		public void DbList2()
		{

			var value = DbValue.FromJson(JsonObject.Parse(@"{ ""L"": [ { ""N"": ""1.1"" }, { ""N"":""7.543"" } ] }"));

			Assert.Equal(DbValueType.L, value.Kind);

			Assert.Equal(new[] { 1.1f, 7.543f }, value.ToArray<float>());
		}
	}
}
