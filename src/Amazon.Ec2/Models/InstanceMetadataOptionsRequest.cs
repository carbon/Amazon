#nullable disable

using System;

namespace Amazon.Ec2
{
    public sealed class InstanceMetadataOptionsRequest
    {
        public InstanceMetadataOptionsRequest() { }

        /// <summary>
        /// This parameter enables or disables the HTTP metadata endpoint on your instances. If the parameter is not specified, the default state is enabled.
        /// Valid values are disabled | enabled
        /// </summary>
        public string HttpEndpoint { get; set; }

        /// <summary>
        /// The desired HTTP PUT response hop limit for instance metadata requests. The larger the number, the further instance metadata requests can travel.
        /// Default: 1, Valid Values 1-64
        /// </summary>
        public int HttpPutResponseHopLimit { get; set; }

        /// <summary>
        /// The state of token usage for your instance metadata requests. If the parameter is not specified in the request, the default state is optional.
        /// If the state is optional, you can choose to retrieve instance metadata with or without a signed token header on your request.If you retrieve the IAM role credentials without a token, the version 1.0 role credentials are returned.If you retrieve the IAM role credentials using a valid signed token, the version 2.0 role credentials are returned.
        /// If the state is required, you must send a signed token header with any instance metadata retrieval requests.In this state, retrieving the IAM role credentials always returns the version 2.0 credentials; the version 1.0 credentials are not available.
        /// Valid values are optional | required
        /// </summary>
        public string HttpTokens { get; set; }
    }
}