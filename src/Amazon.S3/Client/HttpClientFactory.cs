using System.Net.Http;

namespace Amazon.S3;

internal sealed class HttpClientFactory
{
    public static HttpClient Create()
    {
        return new HttpClient(new SocketsHttpHandler {
            ConnectTimeout = TimeSpan.FromSeconds(5),
        //  AutomaticDecompression = DecompressionMethods.All,
            UseCookies = false
        })
        {
            DefaultRequestHeaders = {
                { "User-Agent", "Carbon/3.1" }
            }
        };
    }
}