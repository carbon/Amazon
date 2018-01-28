using System;

namespace Amazon.Rds
{
    public class GetAuthenticationTokenRequest
    {
        public GetAuthenticationTokenRequest(string hostname, int port, string userName)
        {
            HostName = hostname ?? throw new ArgumentNullException(nameof(hostname));
            Port     = port;
            UserName = userName;
        }

        public string HostName { get; }

        public int Port { get; }

        public string UserName { get; }
    }
}