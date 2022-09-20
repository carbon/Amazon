using System.Globalization;

namespace Amazon.Ec2;

public abstract class DescribeRequest
{
    public int? MaxResults { get; set; }

    public string? NextToken { get; set; }

    public List<Filter> Filters { get; } = new();

    protected void AddIds(List<KeyValuePair<string, string>> parameters, string prefix, IReadOnlyList<string>? ids)
    {
        if (ids is null) return;

        for (int i = 0; i < ids.Count; i++)
        {
            int number = i + 1;

            // e.g. VpcId.1
            parameters.Add(new (string.Create(CultureInfo.InvariantCulture, $"{prefix}.{number}"), ids[i]));
        }
    }

    protected List<KeyValuePair<string, string>> GetParameters(string actionName)
    {
        var parameters = new List<KeyValuePair<string, string>> {
            new ("Action", actionName)
        };

        uint number = 1;

        foreach (Filter filter in Filters)
        {
            parameters.Add(new (string.Create(CultureInfo.InvariantCulture, $"Filter.{number}.Name"),  filter.Name));
            parameters.Add(new (string.Create(CultureInfo.InvariantCulture, $"Filter.{number}.Value"), filter.Value));

            number++;
        }

        if (MaxResults is int maxResults)
        {
            parameters.Add(new ("MaxResults", maxResults.ToString(CultureInfo.InvariantCulture)));
        }

        if (NextToken is not null)
        {
            parameters.Add(new ("NextToken", NextToken));
        }

        return parameters;
    }
}
