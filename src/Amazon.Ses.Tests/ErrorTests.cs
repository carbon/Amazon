
using Xunit;

namespace Amazon.Ses.Tests
{
    public class ErrorTests
    {

        [Fact]
        public void ParseError()
        {
            var text = @"<ErrorResponse xmlns=""http://ses.amazonaws.com/doc/2010-12-01/"">
  <Error>
    <Type>Sender</Type>
    <Code>InvalidParameterValue</Code>
    <Message>Local address contains control or whitespace</Message>
  </Error>
  <RequestId>0de719f7-7cde-11e3-8c9d-5942f9840c3a</RequestId>
</ErrorResponse>";

            var response = ErrorResponse.Parse(text);

            Assert.Equal("Sender", response.Error.Type);
            Assert.Equal("InvalidParameterValue", response.Error.Code);
            Assert.Equal("Local address contains control or whitespace", response.Error.Message);

        }
    }
}
