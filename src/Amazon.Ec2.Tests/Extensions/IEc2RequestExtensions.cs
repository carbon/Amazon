namespace Amazon.Ec2;

internal static class IEc2RequestExtensions
{
    public static string Serialize(this IEc2Request request)
    {
        return string.Join('&', request.ToParams().Select(static a => $"{a.Key}={a.Value}"));
    }
}