using System;

namespace Amazon.Rds
{
    public class GetAuthenticationTokenRequest
    {
        public GetAuthenticationTokenRequest() { }

        public GetAuthenticationTokenRequest(string hostname, int port, string userName)
        {
            HostName = hostname;
            Port     = port;
            UserName = userName;
        }

        public string HostName { get; set; }

        public int Port { get; set; }

        public string UserName { get; set; }
    }
}



// http://docs.aws.amazon.com/AmazonRDS/latest/UserGuide/UsingWithRDS.IAMDBAuth.html

// CREATE USER jane_doe IDENTIFIED WITH AWSAuthenticationPlugin as 'RDS';