using System.Text.Json;

namespace Amazon.Ses.Actions.Tests;

public class SendEmailRequestTests
{
    [Fact]
    public void CanSerialize()
    {
        var request = new SendEmailRequest {
            Content = new EmailContent {
                Simple = new Message {
                    Subject = new Content {
                        Data = "Quick brown fox"
                    },
                    Body = new MessageBody {
                        Html = new Content {
                            Data = "Hello"
                        }
                    },
                    Headers = new List<MessageHeader> {
                        new("A", "B")
                    }
                }
            },

            Destination = new Destination {
                ToAddresses = ["test@gmail.com"]
            }
        };

        Assert.Equal(
            """
            {"Content":{"Simple":{"Body":{"Html":{"Data":"Hello"}},"Subject":{"Data":"Quick brown fox"},"Headers":[{"Name":"A","Value":"B"}]}},"Destination":{"ToAddresses":["test@gmail.com"]}}
            """, JsonSerializer.Serialize(request));
    }
}

public class GetAccountResultTests
{
    [Fact]
    public void CanDeserialize()
    {
        var result = JsonSerializer.Deserialize<GetAccountResult>(
            """
            {
              "DedicatedIpAutoWarmupEnabled": true,
              "Details": null,
              "EnforcementStatus": "HEALTHY",
              "ProductionAccessEnabled": true,
              "RecipientValidationAttributes": null,
              "SendQuota":{
                "Max24HourSend": 500000.0,
                "MaxSendRate": 70.0,
                "SentLast24Hours": 62.0
              },
              "SendingEnabled": true,
              "SuppressionAttributes": null,
              "VdmAttributes": null
            }
            """);

        Assert.Equal(500000, result.SendQuota.Max24HourSend);
    }
}
