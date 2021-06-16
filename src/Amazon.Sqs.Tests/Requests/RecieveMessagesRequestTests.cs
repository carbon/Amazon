using System;

using Xunit;

namespace Amazon.Sqs.Tests
{
    public class RecieveMessagesRequestTests
    {
        [Fact]
        public void CanConstruct()
        {
            var request = new RecieveMessagesRequest(5, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(20));

            Assert.Null(request.AttributeNames);
            Assert.Null(request.MessageAttributeNames);
        }
    }
}