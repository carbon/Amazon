#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sts
{
    public sealed class GetCallerIdentityResponse : IStsResponse
    {
        [XmlElement]
        public GetCallerIdentityResult GetCallerIdentityResult { get; set; }
    }

    public class GetCallerIdentityResult
    {
        [XmlElement]
        public string Arn { get; set; }

        [XmlElement]
        public string UserId { get; set; }

        [XmlElement]
        public string Account { get; set; }
    }
}


/*
<GetCallerIdentityResponse xmlns="https://sts.amazonaws.com/doc/2011-06-15/">
  <GetCallerIdentityResult>
   <Arn>arn:aws:iam::123456789012:user/Alice</Arn>
    <UserId>AKIAI44QH8DHBEXAMPLE</UserId>
    <Account>123456789012</Account>
  </GetCallerIdentityResult>
  <ResponseMetadata>
    <RequestId>01234567-89ab-cdef-0123-456789abcdef</RequestId>
  </ResponseMetadata>
</GetCallerIdentityResponse>
*/