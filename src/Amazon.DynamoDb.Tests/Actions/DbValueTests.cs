using System.Net;
using System.Text.Json;

using Carbon.Data.Sequences;

namespace Amazon.DynamoDb.Models.Tests;

public class DbValueTests
{
    private static readonly int[] s_1_2_3i32    = [1, 2, 3];
    private static readonly long[] s_1_2_3i64   = [1L, 2L, 3L];
    private static readonly float[] s_1_2_3f32  = [1f, 2f, 3f];
    private static readonly double[] s_1_2_3f64 = [1d, 2d, 3d];
    private static readonly decimal[] s_1_2_3m  = [1m, 2m, 3m];

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
        var value = JsonSerializer.Deserialize<DbValue>("""{"B":"dmFsdWU="}"""u8);

        Assert.Equal(DbValueType.B, value.Kind);
        Assert.Equal("dmFsdWU=", Convert.ToBase64String(value.ToBinary()));

        Assert.Equal("""{"B":"dmFsdWU="}""", value.ToJsonString());
    }

    [Fact]
    public void DbValueTypes()
    {
        Assert.Equal(DbValueType.B, new DbValue("abc"u8.ToArray()).Kind);
        Assert.Equal(DbValueType.S, new DbValue("hello").Kind);
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

        Assert.Equal(DbValueType.SS, new DbValue((string[])["a", "b", "c"]).Kind);
        Assert.Equal(DbValueType.NS, new DbValue(s_1_2_3i32).Kind);
        Assert.Equal(DbValueType.NS, new DbValue(s_1_2_3i64).Kind);
        Assert.Equal(DbValueType.NS, new DbValue(s_1_2_3f32).Kind);
        Assert.Equal(DbValueType.NS, new DbValue(s_1_2_3f64).Kind);
    }

    [Fact]
    public void SetConversions()
    {
        string[] a_b_c = ["a", "b", "c"];

        Assert.Equal("1,2,3", string.Join(',', new DbValue(s_1_2_3i32).ToSet<Int16>()));
        Assert.Equal("1,2,3", string.Join(',', new DbValue(s_1_2_3i32).ToSet<Int32>()));
        Assert.Equal("1,2,3", string.Join(',', new DbValue(s_1_2_3i32).ToSet<Int64>()));
        Assert.Equal("1,2,3", string.Join(',', new DbValue(s_1_2_3i32).ToSet<float>()));
        Assert.Equal("1,2,3", string.Join(',', new DbValue(s_1_2_3i32).ToSet<double>()));
        Assert.Equal("1,2,3", string.Join(',', new DbValue(s_1_2_3m).ToSet<decimal>()));
        Assert.Equal("a,b,c", string.Join(',', new DbValue(a_b_c).ToStringSet()));
    }

    [Fact]
    public void ArrayConversions()
    {
        Assert.Equal("1,2,3", string.Join(',', new DbValue(s_1_2_3i32).ToArray<Int16>()));
        Assert.Equal("1,2,3", string.Join(',', new DbValue(s_1_2_3i32).ToArray<Int32>()));
        Assert.Equal("1,2,3", string.Join(',', new DbValue(s_1_2_3i32).ToArray<Int64>()));
        Assert.Equal("1,2,3", string.Join(',', new DbValue(s_1_2_3i32).ToArray<float>()));
        Assert.Equal("1,2,3", string.Join(',', new DbValue(s_1_2_3m).ToArray<decimal>()));
        Assert.Equal("1,2,3", string.Join(',', new DbValue(s_1_2_3i32).ToArray<double>()));
    }

    [Fact]
    public void CanDeserializeNumericLists()
    {
        var value = JsonSerializer.Deserialize<DbValue>("""{ "L": [ { "N": "1" }, { "N":"2" } ] }"""u8);

        Assert.Equal(DbValueType.L, value.Kind);

        Assert.Equal([ "1", "2" ], value.ToArray<string>().AsSpan());
        Assert.Equal([ 1, 2 ],     value.ToArray<int>().AsSpan());
        Assert.Equal([ 1L, 2L ],   value.ToArray<long>().AsSpan());
    }

    [Fact]
    public void DbMap()
    {
        var value = new DbValue(new AttributeCollection {
            { "a", 1 },
            { "b", "boat" }
        });

        Assert.Equal("""
            {
              "M": {
                "a": {
                  "N": "1"
                },
                "b": {
                  "S": "boat"
                }
              }
            }
            """, value.ToIndentedJsonString());
    }

    [Fact]
    public void DbMap5()
    {
        var value = new DbValue(AttributeCollection.FromObject(new Machine {
            Id = 1,
            Ips = [
                IPAddress.Parse("192.168.1.1"),
                IPAddress.Parse("192.168.1.2")
            ]
        }));

        Assert.Equal(
            """
            {
              "M": {
                "id": {
                  "N": "1"
                },
                "ips": {
                  "L": [
                    {
                      "B": "wKgBAQ=="
                    },
                    {
                      "B": "wKgBAg=="
                    }
                  ]
                }
              }
            }
            """, value.ToIndentedJsonString());
    }

    [Fact]
    public void DbMap2()
    {
        var value = AttributeCollection.FromObject(new Hi {
            A = new Nested {
                A = 1,
                B = "boat"
            }
        });

        Assert.Equal(
            """
            {
              "a": {
                "M": {
                  "a": {
                    "N": "1"
                  },
                  "b": {
                    "S": "boat"
                  }
                }
              }
            }
            """, value.ToIndentedJsonString());

        Assert.Equal(
            """
            {
              "a": {
                "M": {
                  "a": {
                    "N": "1"
                  },
                  "b": {
                    "S": "boat"
                  }
                }
              }
            }
            """, value.ToIndentedJsonString());

        var a = JsonSerializer.Deserialize<AttributeCollection>(value.ToJsonString()).As<Hi>();

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

        Assert.Equal(
            """
            {
              "id": {
                "B": "ZKWQKAIkHUiA4MGBMfFiGg=="
              }
            }
            """, value.ToIndentedJsonString());

        var a = JsonSerializer.Deserialize<AttributeCollection>(value.ToJsonString()).As<Entity>();

        Assert.Equal(id, a.Id);
    }

    [Fact]
    public void DbMap3()
    {
        var meta = new Meta
        {
            Name = "faces",
            Annotations = [
                new Annotation {
                    Id = 1,
                    Confidence = 1f,
                    Description = "dog",
                    Position = new Position { X = 1, Y = 2, Z = 3 }
                }
            ]
        };

        var value = AttributeCollection.FromObject(meta);

        string json = 
            """
            {
              "name": {
                "S": "faces"
              },
              "annotations": {
                "L": [
                  {
                    "M": {
                      "id": {
                        "N": "1"
                      },
                      "description": {
                        "S": "dog"
                      },
                      "position": {
                        "M": {
                          "x": {
                            "N": "1"
                          },
                          "y": {
                            "N": "2"
                          },
                          "z": {
                            "N": "3"
                          }
                        }
                      },
                      "score": {
                        "N": "0"
                      },
                      "confidence": {
                        "N": "1"
                      },
                      "topicality": {
                        "N": "0"
                      }
                    }
                  }
                ]
              }
            }
            """;

        Assert.Equal(json, value.ToIndentedJsonString());

        var a = JsonSerializer.Deserialize<AttributeCollection>(json);
        var m = a.As<Meta>();

        Assert.Equal("dog", m.Annotations[0].Description);
        Assert.Equal(1f, m.Annotations[0].Position.X);
    }

    [Fact]
    public void DbList2()
    {
        var value = JsonSerializer.Deserialize<DbValue>(
            """
            { 
              "L": [ 
                { "N": "1.1" },
                { "N":"7.543" }
              ]
            }
            """u8);

        Assert.Equal(DbValueType.L, value.Kind);

        Assert.Equal([ 1.1f, 7.543f ], value.ToArray<float>().AsSpan());
    }

    [Fact]
    public void DbMap1()
    {
        var dbValue = Deserialize("""{"M":{"a":{"N":"1"},"b":{"S":"boat"},"c":{"BOOL":true}}}"""u8);

        Assert.Equal(DbValueType.M, dbValue.Kind);

        var attributes = (AttributeCollection)dbValue.Value;

        var a = attributes.Get("a");
        var b = attributes.Get("b");
        var c = attributes.Get("c");

        Assert.Equal(DbValueType.N, a.Kind);
        Assert.Equal(DbValueType.S, b.Kind);
        Assert.Equal(DbValueType.BOOL, c.Kind);
    }

    private static DbValue Deserialize(ReadOnlySpan<byte> text)
    {
        return JsonSerializer.Deserialize<DbValue>(text);
    }
}