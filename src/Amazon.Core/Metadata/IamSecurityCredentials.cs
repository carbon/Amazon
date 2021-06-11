#nullable disable

using System;

namespace Amazon.Metadata
{
    internal sealed class IamSecurityCredentials
    {
        public string Code { get; init; }

        public string Type { get; init; }

        public string AccessKeyId { get; init; }

        public string SecretAccessKey { get; init; }

        public string Token { get; init; }

        public DateTime LastUpdated { get; init; }

        public DateTime Expiration { get; init; }
    }
}

/*
{
  "Code" : "Success",
  "LastUpdated" : "2012-04-26T16:39:16Z",
  "Type" : "AWS-HMAC",
  "AccessKeyId" : "AKIAIOSFODNN7EXAMPLE",
  "SecretAccessKey" : "wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY",
  "Token" : "token",
  "Expiration" : "2012-04-27T22:39:16Z"
}
*/