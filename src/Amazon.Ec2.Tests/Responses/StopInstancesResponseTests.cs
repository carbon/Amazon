namespace Amazon.Ec2.Responses.Tests;

public class StopInstancesResponseTest
{
    [Fact]
    public void CanDeserialize()
    {
        var response = Ec2Serializer<StopInstancesResponse>.Deserialize(
            """
            <StopInstancesResponse xmlns="http://ec2.amazonaws.com/doc/2016-11-15/">
              <requestId>59dbff89-35bd-4eac-99ed-be587EXAMPLE</requestId>
              <instancesSet>
                <item>
                  <instanceId>i-1234567890abcdef0</instanceId>
                  <currentState>
                      <code>64</code>
                      <name>stopping</name>
                  </currentState>
                  <previousState>
                      <code>16</code>
                      <name>running</name>
                  </previousState>
                </item>
              </instancesSet>
            </StopInstancesResponse>
            """);

        Assert.Equal("i-1234567890abcdef0", response.Instances[0].InstanceId);

        Assert.Equal(64, response.Instances[0].CurrentState.Code);
        Assert.Equal("stopping", response.Instances[0].CurrentState.Name);

        Assert.Equal(16, response.Instances[0].PreviousState.Code);
        Assert.Equal("running", response.Instances[0].PreviousState.Name);
    }
}