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
    }
}