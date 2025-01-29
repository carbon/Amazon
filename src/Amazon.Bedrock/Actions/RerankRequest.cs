using System.Text.Json.Serialization;

using Amazon.Bedrock.Models;

namespace Amazon.Bedrock.Actions;

public class RerankRequest
{
    /// <summary>
    /// If the total number of results was greater than could fit in a response, a token is returned in the nextToken field. 
    /// You can enter that token in this field to return the next batch of results.
    /// </summary>
    [JsonPropertyName("nextToken")]
    public string? Token { get; set; }

    [JsonPropertyName("queries")]
    public required IReadOnlyList<RerankQuery> Queries { get; init; }

    [JsonPropertyName("rerankingConfiguration")]
    public required RerankingConfiguration RerankingConfiguration { get; init; }

    [JsonPropertyName("sources")]
    public required IReadOnlyList<RerankSource> Sources { get; init; }
}


public class RerankResult
{
    [JsonPropertyName("nextToken")]
    public string? NextToken { get; set; }

    [JsonPropertyName("results")]
    public required RerankResultItem[] Results { get; set; }
}



public class RerankResultItem
{
    [JsonPropertyName("index")]
    public required int Index { get; set; }

    [JsonPropertyName("relevanceScore")]
    public required float RelevanceScore { get; set; }

    [JsonPropertyName("document")]
    public RerankDocument? Document { get; set; }
}
