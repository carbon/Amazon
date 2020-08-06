using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using Amazon.DynamoDb.JsonConverters;
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

			Assert.Equal(@"{""B"":""dmFsdWU=""}", value.ToSystemTextJson());
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
}", value.ToSystemTextJsonIndented());
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
}", value.ToSystemTextJsonIndented());
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
}", value.ToSystemTextJsonIndented());



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
}", value.ToSystemTextJsonIndented());

            var a = System.Text.Json.JsonSerializer.Deserialize<AttributeCollection>(value.ToSystemTextJson()).As<Hi>();

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
}", value.ToSystemTextJsonIndented());

            var a = System.Text.Json.JsonSerializer.Deserialize<AttributeCollection>(value.ToSystemTextJson()).As<Entity>();

            Assert.Equal(id, a.Id);

        }

        [Fact]
        public void DbMap3()
        {
            var meta = new Meta
            {
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

            string json = @"{
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
}";

            Assert.Equal(json, value.ToSystemTextJsonIndented());


            var a = System.Text.Json.JsonSerializer.Deserialize<AttributeCollection>(json);
            var m = a.As<Meta>();

            Assert.Equal("dog", m.Annotations[0].Description);
            Assert.Equal(1f, m.Annotations[0].Position.X);

        }


        [Fact]
		public void DbList2()
		{

			var value = System.Text.Json.JsonSerializer.Deserialize<DbValue>(@"{ ""L"": [ { ""N"": ""1.1"" }, { ""N"":""7.543"" } ] }");

			Assert.Equal(DbValueType.L, value.Kind);

			Assert.Equal(new[] { 1.1f, 7.543f }, value.ToArray<float>());
		}

        [Fact]
        public void ComplexAttributeCollection()
        {
            var json = @"{""Attributes"":{""PP"":{""B"":""AA==""},""CST"":{""BOOL"":false},""PT"":{""N"":""0""},""LR"":{""B"":""AA==""},""SRol"":{""L"":[{""S"":""NULL""}]},""ODPr"":{""L"":[{""M"":{""Pro"":{""N"":""-1""},""Slo"":{""N"":""0""}}},{""M"":{""Pro"":{""N"":""-1""},""Slo"":{""N"":""1""}}},{""M"":{""Pro"":{""N"":""-1""},""Slo"":{""N"":""2""}}},{""M"":{""Pro"":{""N"":""-1""},""Slo"":{""N"":""3""}}},{""M"":{""Pro"":{""N"":""-1""},""Slo"":{""N"":""4""}}},{""M"":{""Pro"":{""N"":""-1""},""Slo"":{""N"":""5""}}},{""M"":{""Pro"":{""N"":""-1""},""Slo"":{""N"":""6""}}},{""M"":{""Pro"":{""N"":""-1""},""Slo"":{""N"":""7""}}}]},""HS"":{""S"":""08d40421-871b-4bc2-9c5d-d0915cd9a839""},""DDPr"":{""L"":[{""M"":{""Pro"":{""N"":""-1""},""Slo"":{""N"":""0""}}},{""M"":{""Pro"":{""N"":""-1""},""Slo"":{""N"":""1""}}},{""M"":{""Pro"":{""N"":""-1""},""Slo"":{""N"":""2""}}},{""M"":{""Pro"":{""N"":""-1""},""Slo"":{""N"":""3""}}},{""M"":{""Pro"":{""N"":""-1""},""Slo"":{""N"":""4""}}},{""M"":{""Pro"":{""N"":""-1""},""Slo"":{""N"":""5""}}},{""M"":{""Pro"":{""N"":""-1""},""Slo"":{""N"":""6""}}},{""M"":{""Pro"":{""N"":""-1""},""Slo"":{""N"":""7""}}}]},""SOI"":{""N"":""0""},""CVC"":{""N"":""0""},""FT"":{""N"":""0""},""DDCa"":{""L"":[{""S"":""NULL""}]},""CCT"":{""L"":[{""S"":""NULL""}]},""UF"":{""N"":""0""},""FTL"":{""L"":[{""S"":""NULL""}]},""US"":{""B"":""AA==""},""A"":{""BOOL"":false},""SR"":{""N"":""1""},""C"":{""N"":""0""},""DDE"":{""N"":""0""},""F"":{""L"":[{""S"":""NULL""}]},""LasT"":{""N"":""0""},""CFP"":{""N"":""0""},""CP"":{""N"":""0""},""VC"":{""N"":""0""},""DvD"":{""N"":""18463""},""CT"":{""N"":""0""},""SPR"":{""L"":[{""S"":""NULL""}]},""PF"":{""N"":""0""},""SPT"":{""M"":{""NULL"":{""S"":""NULL""}}},""EXPr"":{""L"":[{""M"":{""T"":{""N"":""3""},""V"":{""B"":""AA==""},""Ve"":{""B"":""AA==""}}}]},""VP"":{""L"":[{""M"":{""I"":{""B"":""AQ==""},""C"":{""B"":""AA==""},""L"":{""N"":""0""},""F"":{""N"":""1595261267""}}},{""M"":{""I"":{""B"":""AA==""},""C"":{""B"":""AA==""},""L"":{""N"":""0""},""F"":{""N"":""1595261270""}}},{""M"":{""I"":{""B"":""Ag==""},""C"":{""B"":""AA==""},""L"":{""N"":""0""},""F"":{""N"":""0""}}}]},""Id"":{""S"":""41551""},""RM"":{""BOOL"":false},""PM"":{""BOOL"":false}}}";

            var attrCollection = System.Text.Json.JsonSerializer.Deserialize<UpdateItemResult>(json).Attributes;

            Assert.Equal(-1, (attrCollection["ODPr"].ToArray<DbValue>()[0].Value as AttributeCollection)["Pro"].ToInt());
        }

        [Fact]
        public void ComplexAttributeCollection2()
        {
            var json = @"{""Attributes"":{""LSI"":{""N"":""0""},""Qr"":{""B"":""AA==""},""DN"":{""BOOL"":true},""HS"":{""S"":""0413eeb6-d30e-4384-a753-10921f3b57e0""},""GIT"":{""S"":""NULL""},""v"":{""BOOL"":true},""CNC"":{""N"":""0""},""Pe"":{""N"":""0""},""New"":{""BOOL"":true},""Name"":{""S"":""NULL""},""SQ"":{""N"":""1595261265""},""GId"":{""N"":""-1""},""CD"":{""N"":""1595275664""},""D"":{""N"":""0""},""E"":{""L"":[{""N"":""12""},{""N"":""13""},{""N"":""1""},{""N"":""3""},{""N"":""24""},{""N"":""21""}]},""Ps"":{""L"":[{""S"":""NULL""}]},""AE"":{""N"":""0""},""GL"":{""N"":""0""},""Pay"":{""BOOL"":false},""LJG"":{""S"":""NULL""},""M"":{""N"":""0""},""LJM"":{""S"":""NULL""},""Ds"":{""L"":[{""M"":{""P"":{""M"":{""NULL"":{""S"":""NULL""}}},""I"":{""L"":[{""N"":""-1""},{""N"":""-1""},{""N"":""-1""},{""N"":""-1""},{""N"":""-1""},{""N"":""-1""},{""N"":""-1""}]},""C"":{""L"":[{""N"":""2""},{""N"":""88""},{""N"":""89""},{""N"":""46""},{""N"":""1""},{""N"":""6""},{""N"":""14""}]},""T"":{""N"":""0""}}},{""M"":{""P"":{""M"":{""NULL"":{""S"":""NULL""}}},""I"":{""L"":[{""N"":""-1""},{""N"":""-1""},{""N"":""-1""},{""N"":""-1""},{""N"":""-1""},{""N"":""-1""},{""N"":""-1""}]},""C"":{""L"":[{""N"":""2""},{""N"":""88""},{""N"":""89""},{""N"":""46""},{""N"":""1""},{""N"":""6""},{""N"":""14""}]},""T"":{""N"":""1""}}},{""M"":{""P"":{""M"":{""NULL"":{""S"":""NULL""}}},""I"":{""L"":[{""N"":""-1""},{""N"":""-1""},{""N"":""-1""},{""N"":""-1""},{""N"":""-1""},{""N"":""-1""},{""N"":""-1""}]},""C"":{""L"":[{""N"":""2""},{""N"":""88""},{""N"":""89""},{""N"":""46""},{""N"":""1""},{""N"":""6""},{""N"":""14""}]},""T"":{""N"":""2""}}},{""M"":{""P"":{""M"":{""NULL"":{""S"":""NULL""}}},""I"":{""L"":[{""N"":""-1""},{""N"":""-1""},{""N"":""-1""},{""N"":""-1""},{""N"":""-1""},{""N"":""-1""},{""N"":""-1""}]},""C"":{""L"":[{""N"":""2""},{""N"":""88""},{""N"":""89""},{""N"":""46""},{""N"":""1""},{""N"":""6""},{""N"":""14""}]},""T"":{""N"":""3""}}},{""M"":{""P"":{""M"":{""NULL"":{""S"":""NULL""}}},""I"":{""L"":[{""N"":""-1""},{""N"":""-1""},{""N"":""-1""},{""N"":""-1""},{""N"":""-1""},{""N"":""-1""},{""N"":""-1""}]},""C"":{""L"":[{""N"":""2""},{""N"":""88""},{""N"":""89""},{""N"":""46""},{""N"":""1""},{""N"":""6""},{""N"":""14""}]},""T"":{""N"":""4""}}},{""M"":{""P"":{""M"":{""NULL"":{""S"":""NULL""}}},""I"":{""L"":[{""N"":""-1""},{""N"":""-1""},{""N"":""-1""},{""N"":""-1""},{""N"":""-1""},{""N"":""-1""},{""N"":""-1""}]},""C"":{""L"":[{""N"":""2""},{""N"":""88""},{""N"":""89""},{""N"":""46""},{""N"":""1""},{""N"":""6""},{""N"":""14""}]},""T"":{""N"":""5""}}}]},""Q"":{""L"":[{""S"":""NULL""}]},""CS"":{""L"":[{""S"":""NULL""}]},""Sc"":{""N"":""0""},""AS"":{""N"":""0""},""Id"":{""S"":""41551""},""LJ"":{""N"":""0""}}}";

            var attrCollection = System.Text.Json.JsonSerializer.Deserialize<UpdateItemResult>(json).Attributes;

            var ds0 = attrCollection["Ds"].ToArray<DbValue>()[0].Value as AttributeCollection;
            var c0 = ds0["C"].ToArray<DbValue>()[0];
            var c1 = ds0["C"].ToArray<DbValue>()[1];
            var c2 = ds0["C"].ToArray<DbValue>()[2];
            var c3 = ds0["C"].ToArray<DbValue>()[3];

            Assert.Equal(2, c0.ToInt());
            Assert.Equal(88, c1.ToInt());
            Assert.Equal(89, c2.ToInt());
            Assert.Equal(46, c3.ToInt());
        }

        
    }

    public static class DynamoTestHelper
    {
        public static JsonSerializerOptions IndentedSerializerOptions;
        public static JsonSerializerOptions SerializerOptions;

        static DynamoTestHelper()
        {
            IndentedSerializerOptions = new JsonSerializerOptions()
            {
                WriteIndented = true,
                IgnoreNullValues = true,
            };

            SerializerOptions = new JsonSerializerOptions()
            {
                WriteIndented = false,
                IgnoreNullValues = true,
            };
        }

        public static string ToSystemTextJson(this object obj)
        {
            return System.Text.Json.JsonSerializer.Serialize(obj, obj.GetType(), SerializerOptions);
        }

        public static string ToSystemTextJsonIndented(this object obj)
        {
            return System.Text.Json.JsonSerializer.Serialize(obj, obj.GetType(), IndentedSerializerOptions);
        }
    }
}
