using Xunit;

namespace Amazon.DynamoDb
{
    public class DbConvertersTests
    {
        [Fact]
        public void SerializeBoolTest()
        {
            var converter = DbValueConverterFactory.Get(typeof(bool));

            var cv = converter.FromObject(true, null);

            Assert.Equal(DbValueType.BOOL, cv.Kind);
            Assert.True((bool)cv.Value);

            Assert.False((bool)converter.ToObject(new DbValue(0), null));
            Assert.True((bool)converter.ToObject(new DbValue(1), null));

            Assert.False((bool)converter.ToObject(new DbValue(false), null));
            Assert.True((bool)converter.ToObject(new DbValue(true), null));
        }

        [Fact]
        public void SerializeDbValueListTest()
        {
            var converter = DbValueConverterFactory.Get(typeof(DbValue[]));

            var sampleList = new DbValue[2]
            {
                new DbValue(true),
                new DbValue("hello"),
            };

            var converted = converter.FromObject(sampleList, null);
            Assert.Equal(DbValueType.L, converted.Kind);

            DbValue[] backToObject = (DbValue[])converter.ToObject(converted, null);

            Assert.Equal(sampleList[0].Value, backToObject[0].Value);
            Assert.Equal(sampleList[1].Value, backToObject[1].Value);
        }
    }
}