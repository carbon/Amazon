namespace Amazon.Ec2;

public interface IEc2Request
{
    List<KeyValuePair<string, string>> ToParams();
}