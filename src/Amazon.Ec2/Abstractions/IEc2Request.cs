namespace Amazon.Ec2;

public interface IEc2Request
{
    Dictionary<string, string> ToParams();
}