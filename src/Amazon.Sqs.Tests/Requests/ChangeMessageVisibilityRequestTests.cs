namespace Amazon.Sqs.Tests
{
    public class ChangeMessageVisibilityRequestTests
    {
        [Fact]
        public void CanConstruct()
        {
            var request = new ChangeMessageVisibilityRequest("handle", TimeSpan.FromHours(12));

            Assert.Equal("handle", request.ReceiptHandle);
            Assert.Equal(43_200,   request.VisibilityTimeout);
        }

        [Fact]
        public void ConstructorEnsuresVisibilityTimeoutDoesNotExceedRange()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ChangeMessageVisibilityRequest("handle", TimeSpan.FromHours(13)));
            Assert.Throws<ArgumentOutOfRangeException>(() => new ChangeMessageVisibilityRequest("handle", TimeSpan.FromHours(-1)));
        }
    }
}