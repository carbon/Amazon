using System.Text.Json.Serialization;

namespace Amazon.DynamoDb;

[JsonConverter(typeof(JsonStringEnumConverter<SelectEnum>))]
public enum SelectEnum
{
    Unknown = 0,

    /// <summary>
    /// Returns all of the item attributes. For a table, this is the default. 
    /// For an index, this mode causes Amazon DynamoDB to fetch the full item from the table for each matching item in the index.
    /// If the index is configured to project all item attributes, the matching items will not be fetched from the table.
    /// Fetching items from the table incurs additional throughput cost and latency.
    /// </summary>
    ALL_ATTRIBUTES = 1,

    /// <summary>
    /// Allowed only when querying an index. Retrieves all attributes which have been projected into the index. 
    /// If the index is configured to project all attributes, this is equivalent to specifying ALL_ATTRIBUTES.
    /// </summary>
    ALL_PROJECTED_ATTRIBUTES = 2,

    /// <summary>
    /// Returns the number of matching items, rather than the matching items themselves.
    /// </summary>
    COUNT = 3,

    /// <summary>
    /// Returns only the attributes listed in AttributesToGet.
    /// This is equivalent to specifying AttributesToGet without specifying any value for Select.
    /// </summary>
    SPECIFIC_ATTRIBUTES = 4
}
