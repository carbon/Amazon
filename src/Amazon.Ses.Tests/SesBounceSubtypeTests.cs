namespace Amazon.Ses.Tests;

public sealed class SesBounceSubtypeTests
{
    [Fact]
    public void IdsAreFixed()
    {
        Assert.Equal(2, (int)SesBounceSubtype.General);
        Assert.Equal(8, (int)SesBounceSubtype.AttachmentRejected);

    }
}
