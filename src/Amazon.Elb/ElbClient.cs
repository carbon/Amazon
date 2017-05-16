using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Amazon.Elb
{
    public class ElbClient : AwsClient
    {
        public const string Namespace = "http://elasticloadbalancing.amazonaws.com/doc/2015-12-01/";
        public const string Version = "2015-12-01";

        public ElbClient(AwsRegion region, IAwsCredential credential)
            : base(AwsService.Elb, region, credential) { }

        #region Tags

        public Task<AddTagsResponse> AddTagsAsync(AddTagsRequest request)
        {
            return SendAsync<AddTagsResponse>(request);
        }

        public Task<DescribeTagsResponse> DescribeTagsAsync(DescribeTagsRequest request)
        {
            return SendAsync<DescribeTagsResponse>(request);
        }

        public Task<RemoveTagsResponse> RemoveTagsAsync(RemoveTagsRequest request)
        {
            return SendAsync<RemoveTagsResponse>(request);
        }

        #endregion

        #region Target Groups

        public Task<CreateTargetGroupResponse> CreateTargetGroupAsync(CreateTargetGroupRequest request)
        {
            return SendAsync<CreateTargetGroupResponse>(request);
        }

        public Task<DeleteTargetGroupResponse> DeleteTargetGroupAsync(DeleteTargetGroupRequest request)
        {
            return SendAsync<DeleteTargetGroupResponse>(request);
        }

        public Task<DeregisterTargetsResponse> DeregisterTargetsAsync(DeregisterTargetsRequest request)
        {
            return SendAsync<DeregisterTargetsResponse>(request);
        }

        public Task<DescribeTargetGroupAttributesResponse> DescribeTargetGroupAttributesAsync(DescribeTargetGroupAttributesRequest request)
        {
            return SendAsync<DescribeTargetGroupAttributesResponse>(request);
        }

        public Task<DescribeTargetGroupsResponse> DescribeTargetGroupsAsync(DescribeTargetGroupsRequest request)
        {
            return SendAsync<DescribeTargetGroupsResponse>(request);
        }

        public Task<DescribeTargetHealthResponse> DescribeTargetHealthAsync(DescribeTargetHealthRequest request)
        {
            return SendAsync<DescribeTargetHealthResponse>(request);
        }

        public Task<ModifyTargetGroupAttributesResponse> ModifyTargetGroupAttributesAsync(ModifyTargetGroupAttributesRequest request)
        {
            return SendAsync<ModifyTargetGroupAttributesResponse>(request);
        }

        public Task<RegisterTargetsResponse> RegisterTargetsAsync(RegisterTargetsRequest request)
        {
            return SendAsync<RegisterTargetsResponse>(request);
        }

        public Task<ModifyTargetGroupResponse> ModifyTargetGroupAsync(ModifyTargetGroupRequest request)
        {
            return SendAsync<ModifyTargetGroupResponse>(request);
        }

        #endregion

        #region Listeners

        public Task<CreateListenerResponse> CreateListenerAsync(CreateListenerRequest request)
        {
            return SendAsync<CreateListenerResponse>(request);
        }

        public Task<DeleteListenerResponse> DeleteListenerAsync(DeleteListenerRequest request)
        {
            return SendAsync<DeleteListenerResponse>(request);
        }

        public Task<DescribeListenersResponse> DescribeListenersAsync(DescribeListenersRequest request)
        {
            return SendAsync<DescribeListenersResponse>(request);
        }

        public Task<ModifyListenerResponse> ModifyListenerAsync(ModifyListenerRequest request)
        {
            return SendAsync<ModifyListenerResponse>(request);
        }

        #endregion

        #region Rules

        public Task<CreateRuleResponse> CreateRuleAsync(CreateRuleRequest request)
        {
            return SendAsync<CreateRuleResponse>(request);
        }

        public Task<DeleteRuleResponse> DeleteRuleAsync(DeleteRuleRequest request)
        {
            return SendAsync<DeleteRuleResponse>(request);
        }

        public Task<DescribeRulesResponse> DescribeRulesAsync(DescribeRulesRequest request)
        {
            return SendAsync<DescribeRulesResponse>(request);
        }

        public Task<ModifyRuleResponse> ModifyRuleAsync(ModifyRuleRequest request)
        {
            return SendAsync<ModifyRuleResponse>(request);
        }

        public Task<SetRulePrioritiesResponse> SetRulePrioritiesAsync(SetRulePrioritiesRequest request)
        {
            return SendAsync<SetRulePrioritiesResponse>(request);
        }

        #endregion

        #region Load Balancers

        public Task<CreateLoadBalancerResponse> CreateLoadBalancerAsync(CreateLoadBalancerRequest request)
        {
            return SendAsync<CreateLoadBalancerResponse>(request);
        }

        public Task<DeleteLoadBalancerResponse> DeleteLoadBalancerAsync(DeleteLoadBalancerRequest request)
        {
            return SendAsync<DeleteLoadBalancerResponse>(request);
        }

        public Task<DescribeLoadBalancerAttributesResponse> DescribeLoadBalancerAttributesAsync(DescribeLoadBalancerAttributesRequest request)
        {
            return SendAsync<DescribeLoadBalancerAttributesResponse>(request);
        }

        public Task<DescribeLoadBalancersResponse> DescribeLoadBalancersAsync(DescribeLoadBalancersRequest request)
        {
            return SendAsync<DescribeLoadBalancersResponse>(request);
        }

        public Task<ModifyLoadBalancerAttributesResponse> ModifyLoadBalancerAttributesAsync(ModifyLoadBalancerAttributesRequest request)
        {
            return SendAsync<ModifyLoadBalancerAttributesResponse>(request);
        }

        // Specific Load Balancer Actions ---

        public Task<SetIpAddressTypeResponse> SetIpAddressTypeAsync(SetIpAddressTypeRequest request)
        {
            return SendAsync<SetIpAddressTypeResponse>(request);
        }

        public Task<SetSecurityGroupsResponse> SetSecurityGroupsAsync(SetSecurityGroupsRequest request)
        {
            return SendAsync<SetSecurityGroupsResponse>(request);
        }

        public Task<SetSubnetsResponse> SetSubnetsAsync(SetSubnetsRequest request)
        {
            return SendAsync<SetSubnetsResponse>(request);
        }

        #endregion

        #region SSL Policies

        public Task<DescribeSSLPoliciesResponse> DescribeSSLPoliciesAsync(DescribeSSLPoliciesRequest request)
        {
            return SendAsync<DescribeSSLPoliciesResponse>(request);
        }

        #endregion




        #region Helpers

        private async Task<T> SendAsync<T>(IElbRequest request)
            where T: IElbResponse
        {
            var parameters = RequestHelper.ToParams(request);

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, Endpoint)
            {
                Content = GetPostContent(parameters)
            };

            var responseText = await base.SendAsync(httpRequest).ConfigureAwait(false);

            return ElbResponseHelper<T>.ParseXml(responseText);
        }

        private FormUrlEncodedContent GetPostContent(Dictionary<string, string> parameters)
        {
            parameters.Add("Version", Version);

            return new FormUrlEncodedContent(parameters);
        }

        #endregion
    }
}
