#nullable enable

using System;
using System.Collections.Generic;

namespace Amazon.Ec2
{
    public abstract class DescribeRequest
    {
        public int? MaxResults { get; set; }

        public string? NextToken { get; set; }

        public List<Filter> Filters { get; } = new List<Filter>();

        protected void AddIds(Dictionary<string, string> parameters, string prefix, IReadOnlyList<string>? ids)
        {
            if (ids is null) return;

            for (var i = 0; i < ids.Count; i++)
            {
                // e.g. VpcId.1
                parameters.Add(prefix + "." + (i + 1), ids[i]);
            }
        }

        protected Dictionary<string, string> GetParameters(string actionName)
        {
            if (actionName is null)
                throw new ArgumentNullException(nameof(actionName));

            var parameters = new Dictionary<string, string> {
                { "Action", actionName }
            };

            var i = 1;

            foreach (Filter filter in Filters)
            {
                string prefix = "Filter." + i + ".";

                parameters.Add(prefix + "Name", filter.Name);
                parameters.Add(prefix + "Value", filter.Value);

                i++;
            }

            if (MaxResults != null)
            {
                parameters.Add("MaxResults", MaxResults.Value.ToString());
            }

            if (NextToken != null)
            {
                parameters.Add("NextToken", NextToken);
            }

            return parameters;
        }
    }
}
