using System.Text;

namespace Amazon.Ec2;

internal static class IEc2RequestExtensions
{
    public static string Serialize(this IEc2Request request)
    {
        var sb = new StringBuilder();

        int i = 0;

        foreach (var (key, value) in request.ToParams())
        {
            if (i > 0) sb.Append('&');

            sb.Append(key);
            sb.Append('=');
            sb.Append(value);

            i++;
        }

        return sb.ToString();
    }
}