using System.Text.Json.Serialization;

namespace Cohere;

[JsonConverter(typeof(JsonStringEnumConverter<EmbedV4InputType>))]
public enum EmbedV4InputType
{
    /// <summary>
    /// Used for embeddings stored in a vector database for search use-cases.
    /// </summary>
    [JsonStringEnumMemberName("search_document")]
    SearchDocument = 1,

    /// <summary>
    /// Used for embeddings of search queries run against a vector DB to find relevant documents.
    /// </summary>
    [JsonStringEnumMemberName("search_query")]
    SearchQuery = 2,

    /// <summary>
    /// Used for embeddings passed through a text classifier.
    /// </summary>
    [JsonStringEnumMemberName("classification")]
    Classification = 3,

    /// <summary>
    /// Used for the embeddings run through a clustering algorithm.
    /// </summary>
    [JsonStringEnumMemberName("clustering")]
    Clustering = 4
}
