﻿using Carbon.Data;

namespace Amazon.DynamoDb.Converters;

public sealed class AttributeCollectionConverter : IDbValueConverter
{
    public DbValue FromObject(object value, IMember? member) => new DbValue((AttributeCollection)value);

    public object ToObject(DbValue item, IMember? member) => (AttributeCollection)item.Value;
}