using System.Collections.ObjectModel;

namespace Amazon.DynamoDb;

public sealed class TableItemCollection(string name) : Collection<AttributeCollection>
{
    public string Name { get; } = name ?? throw new ArgumentNullException(nameof(name));
}