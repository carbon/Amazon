using System;
using Carbon.Json;

using Xunit;

namespace Amazon.Ssm.Tests
{
    public class CommandTests
    {
        [Fact]
        public void ParseCommand()
        {
            var text = @"{ ""CommandId"":""id"",""Comment"":"""",""CompletedCount"":0,""DocumentName"":""update_app4"",""ErrorCount"":0,""ExpiresAfter"":1.494832672676E9,""InstanceIds"":[],""MaxConcurrency"":""50"",""MaxErrors"":""0"",""NotificationConfig"":{""NotificationArn"":"""",""NotificationEvents"":[],""NotificationType"":""""},""OutputS3BucketName"":"""",""OutputS3KeyPrefix"":"""",""Parameters"":{""appName"":[""platform""]},""RequestedDateTime"":1.494825472676E9,""ServiceRole"":"""",""Status"":""Pending"",""StatusDetails"":""Pending"",""TargetCount"":0,""Targets"":[{""Key"":""tag: envId"",""Values"":[""1""]}]}";

            var command = JsonObject.Parse(text).As<Command>();


            Assert.Equal(1494825472676, new DateTimeOffset(command.RequestedDateTime).ToUnixTimeMilliseconds());
        }
    }
}
