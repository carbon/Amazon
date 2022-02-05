#nullable enable

using System.Globalization;

namespace Amazon.Ec2;

public abstract class DescribeRequest
{
    public int? MaxResults { get; set; }

    public string? NextToken { get; set; }

    public List<Filter> Filters { get; } = new();

    protected void AddIds(Dictionary<string, string> parameters, string prefix, IReadOnlyList<string>? ids)
    {
        if (ids is null) return;

        for (int i = 0; i < ids.Count; i++)
        {
            int number = i + 1;

            // e.g. VpcId.1
            parameters.Add(string.Create(CultureInfo.InvariantCulture, $"{prefix}.{number}"), ids[i]);
        }
    }

    protected Dictionary<string, string> GetParameters(string actionName)
    {
        var parameters = new Dictionary<string, string> {
            { "Action", actionName }
        };

        int number = 1;

        foreach (Filter filter in Filters)
        {
            parameters.Add(string.Create(CultureInfo.InvariantCulture, $"Filter.{number}.Name"),  filter.Name);
            parameters.Add(string.Create(CultureInfo.InvariantCulture, $"Filter.{number}.Value"), filter.Value);

            number++;
        }

        if (MaxResults is int maxResults)
        {
            parameters.Add("MaxResults", maxResults.ToString(CultureInfo.InvariantCulture));
        }

        if (NextToken != null)
        {
            parameters.Add("NextToken", NextToken);
        }

        return parameters;
    }
}
