using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;

using Amazon.DynamoDb.Serialization;

using Carbon.Data;

namespace Amazon.DynamoDb;

[JsonConverter(typeof(AttributeCollectionJsonConverter))]
public sealed class AttributeCollection : IEnumerable<KeyValuePair<string, DbValue>>, IReadOnlyDictionary<string, DbValue>
{
    private readonly Dictionary<string, DbValue> _items;

    public AttributeCollection(int capacity)
    {
        _items = new Dictionary<string, DbValue>(capacity);
    }

    public AttributeCollection()
    {
        _items = new Dictionary<string, DbValue>();
    }

    #region Add Helpers

    public void Add(string name, string value)
        => _items.Add(name, new DbValue(value));

    public void Add(string name, short value)
        => _items.Add(name, new DbValue(value));

    public void Add(string name, int value)
        => _items.Add(name, new DbValue(value));

    public void Add(string name, long value)
        => _items.Add(name, new DbValue(value));

    public void Add(string name, float value)
        => _items.Add(name, new DbValue(value));

    public void Add(string name, double value)
        => _items.Add(name, new DbValue(value));

    public void Add(string name, string[] strings)
        => _items.Add(name, new DbValue(strings));

    public void Add(string name, int[] numbers)
        => _items.Add(name, new DbValue(numbers));

    public void Add(string name, byte[] data)
        => _items.Add(name, new DbValue(data));

    public void Add(string name, DbValue value)
        => _items.Add(name, value);

    #endregion

    public int Count => _items.Count;

    #region IReadOnlyDictionary

    IEnumerable<string> IReadOnlyDictionary<string, DbValue>.Keys => _items.Keys;

    IEnumerable<DbValue> IReadOnlyDictionary<string, DbValue>.Values => _items.Values;

    #endregion

    public bool TryGet(string name, out DbValue value)
    {
        return _items.TryGetValue(name, out value);
    }

    public bool ContainsKey(string name) => _items.ContainsKey(name);

    public DbValue Get(string name)
    {
        _items.TryGetValue(name, out DbValue value);

        return value;
    }

    public HashSet<string>? GetStringSet(string key)
    {
        return TryGet(key, out DbValue value)
            ? value.ToStringSet()
            : null;
    }

    public byte[]? GetBinary(string key)
    {
        return TryGet(key, out DbValue value)
            ? value.ToBinary()
            : null;
    }

    public int GetInt(string key)
    {
        return TryGet(key, out DbValue value)
            ? value.ToInt()
            : 0;
    }

    public string? GetString(string key)
    {
        return TryGet(key, out DbValue value)
            ? value.ToString()
            : null;
    }

    public void Set(string name, DbValue value)
    {
        _items[name] = value;
    }

    public static AttributeCollection FromJsonElement(in JsonElement json)
    {
        return JsonSerializer.Deserialize(json, DynamoDbSerializationContext.Default.AttributeCollection)!;
    }

    public DbValue this[string key]
    {
        get => Get(key);
        set => Set(key, value);
    }

    #region IEnumerable<KeyValuePair<string,DbValue>> Members

    IEnumerator<KeyValuePair<string, DbValue>> IEnumerable<KeyValuePair<string, DbValue>>.GetEnumerator()
        => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();

    #endregion

    #region Helpers

    public static AttributeCollection FromObject(object instance)
    {
        return FromObject(instance, DatasetInfo.Get(instance.GetType()));
    }

    internal static AttributeCollection FromObject(object instance, DatasetInfo schema)
    {
        var attributes = new AttributeCollection();

        foreach (MemberDescriptor member in schema.Members)
        {
            var value = member.GetValue(instance);

            if (value is null) continue;

            var typeInfo = TypeDetails.Get(member.Type);

            if (DbValueConverterFactory.TryGet(member.Type, out IDbValueConverter? converter))
            {
                var dbValue = converter.FromObject(value, member);

                if (dbValue.Kind == DbValueType.Unknown) continue;

                attributes.Add(member.Name, dbValue);
            }
            else if (member.Type.IsArray)
            {
                var a = (Array)value;

                var list = new DbValue[a.Length];

                for (int i = 0; i < list.Length; i++)
                {
                    list[i] = new DbValue(FromObject(a.GetValue(i)!));
                }

                attributes.Add(member.Name, new DbValue(list));
            }
            else if (typeInfo.IsList && typeInfo.ElementType is not null)
            {
                IList a = (IList)value;

                var list = new DbValue[a.Count];

                if (DbValueConverterFactory.TryGet(typeInfo.ElementType, out IDbValueConverter? c))
                {
                    for (int i = 0; i < list.Length; i++)
                    {
                        list[i] = c.FromObject(a[i]!, member);
                    }
                }
                else
                {
                    for (int i = 0; i < list.Length; i++)
                    {
                        list[i] = new DbValue(FromObject(a[i]!));
                    }
                }

                attributes.Add(member.Name, new DbValue(list));
            }
            else
            {
                try
                {
                    attributes.Add(member.Name, new DbValue(FromObject(value)));
                }
                catch
                {
                    throw new Exception($"{value.GetType().Name}/{typeInfo.IsList}");
                }
            }
        }

        return attributes;
    }

    public KeyValuePair<string, object>[] ToKey()
    {
        var keyItems = new KeyValuePair<string, object>[_items.Count];
        int i = 0;

        foreach (var attribute in _items)
        {
            keyItems[i] = new(attribute.Key, attribute.Value.ToPrimitiveValue());

            i++;
        }

        return keyItems;
    }

    public T As<T>() where T : notnull => As<T>(DatasetInfo.Get<T>());

    internal T As<T>(DatasetInfo model)
        where T : notnull
    {
        T instance = Activator.CreateInstance<T>();

        foreach (var attribute in _items)
        {
            MemberDescriptor member = model[attribute.Key]!;

            if (member is null) continue;

            if (DbValueConverterFactory.TryGet(member.Type, out IDbValueConverter? converter))
            {
                var value = converter.ToObject(attribute.Value, member);

                member.SetValue(instance, value);
            }
            else if (attribute.Value.Kind == DbValueType.M)
            {
                var map = (AttributeCollection)attribute.Value.Value;

                member.SetValue(instance, map.DeserializeMap(DatasetInfo.Get(member.Type)));

                continue;
            }
            else if (member.Type.IsArray)
            {
                var list = DeserializeArray(member.Type.GetElementType()!, (DbValue[])attribute.Value.Value);

                member.SetValue(instance, list);
            }
        }

        return instance;
    }

    private static Array DeserializeArray(Type elementType, DbValue[] values)
    {
        var elementModel = DatasetInfo.Get(elementType);

        var list = Array.CreateInstance(elementType, values.Length);

        for (int i = 0; i < values.Length; i++)
        {
            ref DbValue value = ref values[i];

            if (value.Kind == DbValueType.M)
            {
                list.SetValue(((AttributeCollection)value.Value).DeserializeMap(elementModel), i);
            }
        }

        return list;
    }

    private object DeserializeMap(DatasetInfo model)
    {
        object instance = Activator.CreateInstance(model.Type)!;

        foreach (var attribute in _items)
        {
            MemberDescriptor? member = model[attribute.Key];

            if (member is null) continue;

            if (DbValueConverterFactory.TryGet(member.Type, out IDbValueConverter? converter))
            {
                var value = converter.ToObject(attribute.Value, member);

                member.SetValue(instance, value);
            }
            else if (attribute.Value.Kind is DbValueType.M)
            {
                var map = (AttributeCollection)attribute.Value.Value;

                member.SetValue(instance, map.DeserializeMap(DatasetInfo.Get(member.Type)));

                continue;
            }
            else if (member.Type.IsArray)
            {
                var list = DeserializeArray(member.Type.GetElementType()!, (DbValue[])attribute.Value.Value);

                member.SetValue(instance, list);
            }
        }

        return instance;
    }

    bool IReadOnlyDictionary<string, DbValue>.TryGetValue(string key, out DbValue value)
    {
        return _items.TryGetValue(key, out value);
    }

    #endregion
}
