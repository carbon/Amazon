using System;
using System.Text.Json;

using Xunit;

namespace Amazon.Ssm.Tests;

public class CommandTests
{
    [Fact]
    public void ParseCommand()
    {
        var text = """{ "CommandId":"id","Comment":"","CompletedCount":0,"DocumentName":"update_app4","ErrorCount":0,"ExpiresAfter":1.494832672676E9,"InstanceIds":[],"MaxConcurrency":"50","MaxErrors":"0","NotificationConfig":{"NotificationArn":"","NotificationEvents":[],"NotificationType":""},"OutputS3BucketName":"","OutputS3KeyPrefix":"","Parameters":{"appName":["platform"]},"RequestedDateTime":1.494825472676E9,"ServiceRole":"","Status":"Pending","StatusDetails":"Pending","TargetCount":0,"Targets":[{"Key":"tag: envId","Values":["1"]}]}""";

        var command = JsonSerializer.Deserialize<Command>(text);

        Assert.Equal("id", command.CommandId);

        Assert.Equal("update_app4", command.DocumentName);
        
        Assert.Empty(command.Comment);
        Assert.Equal(0, command.CompletedCount);

        Assert.Equal(CommandStatus.Pending, command.Status);
        Assert.Equal("Pending", command.StatusDetails);

        Assert.Equal(1494825472676, ((DateTimeOffset)command.RequestedDateTime).ToUnixTimeMilliseconds());
    }

    [InlineData("InProgress", CommandStatus.InProgress)]
    [InlineData("Cancelled", CommandStatus.Cancelled)]
    [Theory]
    public void ParseStatus(string text, CommandStatus status)
    {
        var command = JsonSerializer.Deserialize<Command>(@$"{{ ""Status"":""{text}"" }}");

        Assert.Equal(status, command.Status);
        Assert.Null(command.RequestedDateTime);
    }
}
