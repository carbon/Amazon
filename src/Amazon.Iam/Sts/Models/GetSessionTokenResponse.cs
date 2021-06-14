using System;
using System.Xml.Linq;

namespace Amazon.Sts.Models
{
    public sealed class GetSessionTokenResponse
    {
        public static AwsSession Parse(string text)
        {
            XElement rootEl = XElement.Parse(text);                              // GetSessionTokenResponse

            var ns = rootEl.Name.Namespace;

            XElement resultEl      = rootEl.Element(ns + "GetSessionTokenResult")!;    // GetSessionTokenResult
            XElement credentialsEl = resultEl.Element(ns + "Credentials")!;

            return new AwsSession(
                sessionToken    : credentialsEl.Element(ns + "SessionToken")!.Value,
                secretAccessKey : credentialsEl.Element(ns + "SecretAccessKey")!.Value,
                expiration      : DateTime.Parse(credentialsEl.Element(ns + "Expiration")!.Value).ToUniversalTime(),
                accessKeyId     : credentialsEl.Element(ns + "AccessKeyId")!.Value
            );
        }
    }
}

/*
<GetSessionTokenResponse xmlns=""https://sts.amazonaws.com/doc/2011-06-15/"">
  <GetSessionTokenResult>
    <Credentials>
      <SessionToken>abc</SessionToken>
      <SecretAccessKey>abc</SecretAccessKey>
      <Expiration>2012-02-15T21:58:01.255Z</Expiration>
      <AccessKeyId>abc</AccessKeyId>
    </Credentials>
  </GetSessionTokenResult>
  <ResponseMetadata>
    <RequestId>bf29b821-5817-11e1-9055-eba9865eb6e8</RequestId>
  </ResponseMetadata>
</GetSessionTokenResponse>
*/
