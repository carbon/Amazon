#nullable disable

using System;

namespace Amazon.Metadata;

internal sealed class IamSecurityCredentials
{
    public string Code { get; set; }

    public string Type { get; set; }

    public string AccessKeyId { get; set; }

    public string SecretAccessKey { get; set; }

    public string Token { get; set; }

    public DateTime LastUpdated { get; set; }

    public DateTime Expiration { get; set; }
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