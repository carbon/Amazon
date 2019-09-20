
using System.Net.Mail;

using Xunit;

namespace Amazon.Ses.Tests
{
    public class SesEmailTests
    {
        [Fact]
        public void ConstructFromMailMessage()
        {
            var message = new MailMessage("from@test.com", "to@test.com", "Subject", "Body");

            var sesEmail = SesEmail.FromMailMessage(message);

            Assert.Equal("from@test.com", sesEmail.Source);
            Assert.Equal(new[] { "to@test.com" }, sesEmail.To);
            Assert.Equal("Body",  sesEmail.Text.Data);
            Assert.Equal("Subject", sesEmail.Subject.Data);
        }

        [Fact]
        public void ConstructFromMailMessage_Complex()
        {
            var message = new MailMessage("from@test.com", "to@test.com", "Subject", "Body") {
                IsBodyHtml = true,
                CC = {
                  "a@test.com",  
                  "b@test.com"
                },
                ReplyToList = {
                    "c@test.com"
                }
            };

            var response = SesEmail.FromMailMessage(message);

            Assert.Equal("from@test.com",                       response.Source);
            Assert.Equal(new[] { "to@test.com" },               response.To);
            Assert.Equal(new[] { "a@test.com", "b@test.com" },  response.CC);
            Assert.Equal(new[] { "c@test.com" },                response.ReplyTo);

            Assert.Equal("Body", response.Html.Data);
            Assert.Equal("Subject", response.Subject.Data);

            var q = response.ToParams();

            Assert.Equal(8, q.Count);
        }
    }
}