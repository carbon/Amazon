namespace Amazon.Ses.Models.Tests;

public class ReceiptModelTests
{
    [Fact]
    public void S3Action_RequiresBucketName()
    {
        Assert.Throws<ArgumentNullException>(() => new S3Action(null, "prefix", null, null));
        Assert.Throws<ArgumentException>(() => new S3Action("", "prefix", null, null));
    }

    [Fact]
    public void S3Action_SetsProperties()
    {
        var action = new S3Action("bucket", "prefix", "kms", "topic");

        Assert.Equal("bucket", action.BucketName);
        Assert.Equal("prefix", action.ObjectKeyPrefix);
        Assert.Equal("kms",    action.KmsKeyArn);
        Assert.Equal("topic",  action.TopicArn);
    }

    [Fact]
    public void RequiredArgumentsAreValidated()
    {
        Assert.Throws<ArgumentNullException>(() => new LambdaAction(null));
        Assert.Throws<ArgumentNullException>(() => new WorkmailAction(null));
        Assert.Throws<ArgumentNullException>(() => new DeleteIdentityRequest(null));
    }

    [Fact]
    public void ReceiptRuleRequests_SetProperties()
    {
        var rule = new ReceiptRule { Name = "rule", Enabled = true };

        var create = new CreateReceiptRuleRequest(rule, "set", after: "previous");
        var update = new UpdateReceiptRuleRequest(rule, "set");

        Assert.Same(rule, create.Rule);
        Assert.Equal("set", create.RuleSetName);
        Assert.Equal("previous", create.After);

        Assert.Same(rule, update.Rule);
        Assert.Equal("set", update.RuleSetName);
    }
}
