using System.Diagnostics.CodeAnalysis;

using Amazon.Helpers;

namespace Amazon.Route53;

public sealed class ListResourceRecordSetsRequest
{
    public ListResourceRecordSetsRequest() { }

    [SetsRequiredMembers]
    public ListResourceRecordSetsRequest(string id)
    {
        Id = id;
    }

    public required string Id { get; set; }

    public string? Identifier { get; set; }

    public int? MaxItems { get; set; }

    public string? Name { get; set; }

    public ResourceRecordType? Type { get; set; }

    public string ToQueryString()
    {
        var dic = new List<KeyValuePair<string, string>>(4);

        if (Identifier != null)
        {
            dic.Add(new("identifier", Identifier));
        }

        if (MaxItems.HasValue)
        {
            dic.Add(new("maxitems", MaxItems.Value.ToString()));
        }

        if (Name != null)
        {
            dic.Add(new("name", Name));
        }

        if (Type.HasValue)
        {
            dic.Add(new("type", Type.Value.ToString()));
        }

        return dic.ToQueryString();
    }
}