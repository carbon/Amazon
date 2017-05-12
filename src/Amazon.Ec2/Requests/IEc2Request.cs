namespace Amazon.Ec2
{
    public interface IEc2Request
    {
        AwsRequest ToParams();
    }
}