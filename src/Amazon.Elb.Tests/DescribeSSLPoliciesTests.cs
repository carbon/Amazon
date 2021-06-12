using Xunit;

namespace Amazon.Elb.Tests
{
    public class DescribeSSLPoliciesTests
    {
        [Fact]
        public void Deserialize()
        {
            var text = $@"<DescribeSSLPoliciesResponse xmlns=""{ElbClient.Namespace}"">
  <DescribeSSLPoliciesResult> 
    <SslPolicies> 
      <member> 
        <Ciphers> 
          <member> 
            <Name>ECDHE-ECDSA-AES128-GCM-SHA256</Name> 
            <Priority>1</Priority> 
          </member> 
          <member> s
            <Name>ECDHE-RSA-AES128-GCM-SHA256</Name> 
            <Priority>2</Priority> 
          </member> 
          <member> 
            <Name>ECDHE-ECDSA-AES128-SHA256</Name> 
            <Priority>3</Priority> 
          </member> 
          <member> 
            <Name>AES256-SHA</Name> 
            <Priority>19</Priority> 
          </member> 
        </Ciphers> 
        <Name>ELBSecurityPolicy-2016-08</Name> 
        <SslProtocols> 
          <member>TLSv1</member> 
          <member>TLSv1.1</member> 
          <member>TLSv1.2</member> 
        </SslProtocols> 
      </member> 
    </SslPolicies> 
  </DescribeSSLPoliciesResult> 
  <ResponseMetadata> 
    <RequestId>a78c9aee-f2aa-11e5-8a24-ffe2bf8623ae</RequestId> 
  </ResponseMetadata>
</DescribeSSLPoliciesResponse>";

            var r = ElbSerializer<DescribeSSLPoliciesResponse>.DeserializeXml(text);

            var policies = r.DescribeSSLPoliciesResult.SslPolicies;

            Assert.Single(policies);

            var policy = policies[0];

            Assert.Equal(4,         policy.Ciphers.Length);
            Assert.Equal(3,         policy.SslProtocols.Length);
            Assert.Equal("TLSv1",   policy.SslProtocols[0]);
            Assert.Equal("TLSv1.1", policy.SslProtocols[1]);

            Assert.Equal("ELBSecurityPolicy-2016-08", policies[0].Name);
        }
    }
}
