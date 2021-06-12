using System.ComponentModel.DataAnnotations;

namespace Amazon.Ec2
{
    public sealed class InstanceMetadataOptionsRequest
    {
        public static readonly InstanceMetadataOptionsRequest RequireHttpToken = new () {
            HttpTokens = "required"
        }; 

        // Valid Values: disabled | enabled
        // Default = enabled
        public string? HttpEndpoint { get; init; }

        /// <summary>
        /// The desired HTTP PUT response hop limit for instance metadata requests. 
        /// The larger the number, the further instance metadata requests can travel.
        /// Default = 1
        /// </summary>
        [Range(1, 64)]
        public int? HttpPutResponseHopLimit { get; init; }

        /// <summary>
        /// The state of token usage for your instance metadata requests. 
        /// If the parameter is not specified in the request, the default state is optional.
        /// optional | required
        /// </summary>
        public string? HttpTokens { get; init; }
    }
}