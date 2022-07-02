using System.Text;

namespace Amazon.Elb.Tests;

public static class Serializer
{
    public static string Serialize(IElbRequest request)
    {
        var sb = new StringBuilder();

        foreach (var (key, value) in RequestHelper.ToParams(request))
        {
            if (sb.Length > 0)
            {
                sb.Append('&');
            }

            sb.Append(key);
            sb.Append('=');
            sb.Append(value);
        }

        return sb.ToString();
    }
}