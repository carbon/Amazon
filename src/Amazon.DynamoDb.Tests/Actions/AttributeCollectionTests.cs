using System;
using System.Text.Json;

namespace Amazon.DynamoDb.Models.Tests
{
    public class AttributeCollectionTests
    {
        [Fact]
        public void DbItem_Enum_Test()
        {
            var item = new AttributeCollection {
                { "Type", 1 }
            };

            var record = item.As<Record>();

            Assert.Equal(RecordType.One, record.Type);
        }

        [Fact]
        public void CanSetReadOnlyProperties()
        {
            var item = new AttributeCollection {
                { "id",   234 },
                { "type", "Three" }
            };

            var record = item.As<ReadOnlyRecord>();

            Assert.Equal(234, record.Id);

            Assert.Equal(RecordType.Three, record.Type);
        }

        [Fact]
        public void DbItem_As_Media_Test()
        {
            var created = new DateTime(2012, 01, 01, 0, 0, 0, DateTimeKind.Utc);

            var item = new AttributeCollection {
                { "Transformation", "resize:500x500.jpeg" },
                { "Duration", 30000 },
                { "Created", new DateTimeOffset(created).ToUnixTimeSeconds() }
            };

            item.Add("Hash", new DbValue("FHX8PLDaGoI2hSlwjI1yFvqWTcQ=", DbValueType.B));

            var media = item.As<Media>();

            Assert.Equal("resize:500x500.jpeg", media.Transformation);
            Assert.Equal(TimeSpan.FromSeconds(30), media.Duration);
            Assert.Equal(Convert.FromBase64String("FHX8PLDaGoI2hSlwjI1yFvqWTcQ="), media.Hash);
            Assert.Equal(created, media.Created);

            var attributes = AttributeCollection.FromObject(media);

            Assert.Equal("resize:500x500.jpeg", attributes["Transformation"].Value.ToString());
            Assert.Equal(DbValueType.N,         attributes["Duration"].Kind);
            Assert.Equal(DbValueType.N,         attributes["Created"].Kind);
        }

        [Fact]
        public void DbItem_As_Person_Test()
        {
            var item = new AttributeCollection {
                { "Name", "Val" },
                { "Age", 100 }
            };

            var person = item.As<Person>();

            Assert.Equal("Val", person.Name);
            Assert.Equal(100, person.Age);
        }

        [Fact]
        public void Db_Item1()
        {
            var item = new AttributeCollection {
                { "A", "alphabet" }
            };

            item.Add("B", "bananas");
            item.Add("C", "candy");
            item.Add("One", 1);
            item.Add("Two", 2);
            item.Add("Three", 3);

            var jsonText = @"{""A"":{""S"":""alphabet""},""B"":{""S"":""bananas""},""C"":{""S"":""candy""},""One"":{""N"":""1""},""Two"":{""N"":""2""},""Three"":{""N"":""3""}}";

            Assert.Equal("alphabet", item.GetString("A"));
            Assert.Equal("bananas", item.GetString("B"));
            Assert.Equal("candy", item.GetString("C"));
            Assert.Null(item.GetString("D"));

            Assert.Equal(jsonText, item.ToSystemTextJson());

            Assert.Equal(jsonText, JsonSerializer.Deserialize<AttributeCollection>(jsonText).ToSystemTextJson());
        }

        [Fact]
        public void Db_Item2()
        {
            var item = JsonSerializer.Deserialize<AttributeCollection>(@"{""hitCount"":{""N"":""225""},""date"":{""S"":""2011-05-31T00:00:00Z""},""siteId"":{""N"":""221051""}}");

            Assert.Equal("225", item["hitCount"].Value);
            Assert.Equal("2011-05-31T00:00:00Z", item["date"].Value);
            Assert.Equal("221051", item["siteId"].Value);
        }
  
        [Fact]
        public void Db_Item3()
        {
            var item = new AttributeCollection {
                { "colors", new[] { "red", "blue", "green" } }
            };

            item.Add("containedIn", new DbValue(new[] { 1, 2, 3 }));

            var jsonText = item.ToSystemTextJson();

            item = JsonSerializer.Deserialize<AttributeCollection>(jsonText);

            Assert.Equal(3, item.GetStringSet("colors").Count);
            Assert.Contains("red", item.GetStringSet("colors"));
            Assert.DoesNotContain("purple", item.GetStringSet("colors"));
            Assert.Equal(3, item.GetStringSet("containedIn").Count);
            Assert.Contains("1", item.GetStringSet("containedIn"));
            Assert.DoesNotContain("4", item.GetStringSet("containedIn"));
        }
  
    
        [Fact]
        public void Db_Item4()
        {
            var item = JsonSerializer.Deserialize<AttributeCollection>(@"{""Boolean"":{""BOOL"":true}}");

            Assert.Equal(DbValueType.BOOL, item["Boolean"].Kind);
            Assert.True((bool)item["Boolean"].Value);

            Assert.True(item.As<Smorsborg>().Boolean);

            Assert.Equal(@"{
  ""Boolean"": {
    ""BOOL"": true
  }
}", item.ToSystemTextJsonIndented());

        }
    }
}
