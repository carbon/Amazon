namespace Amazon.Ses.Tests;

public sealed class BounceTypeTests
{
    [Fact]
    public void IdsAreFixed()
    {
        Assert.Equal(1, (int)SesBounceType.Undetermined);
        Assert.Equal(2, (int)SesBounceType.Permanent);
    }
}