using Xunit;

namespace Amazon.Elb.Tests
{
    public class DescribeSSLPoliciesTests
    {
        [Fact]
        public void A()
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
          <member> 
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

            var r = ElbResponseHelper<DescribeSSLPoliciesResponse>.DeserializeXml(text);

            var policies = r.DescribeSSLPoliciesResult.SslPolicies;

            Assert.Equal(1, policies.Length);

            Assert.Equal(4, policies[0].Ciphers.Length);
            Assert.Equal(3, policies[0].SslProtocols.Length);
            Assert.Equal("TLSv1", policies[0].SslProtocols[0]);
            Assert.Equal("TLSv1.1", policies[0].SslProtocols[1]);

            Assert.Equal("ELBSecurityPolicy-2016-08", policies[0].Name);
        }
    }
}
