using System;
using System.Collections;
using System.Collections.Generic;

using Carbon.Json;
using Carbon.Data;

namespace Amazon.DynamoDb
{
    public class AttributeCollection : IEnumerable<KeyValuePair<string, DbValue>>
    {
        private readonly Dictionary<string, DbValue> items = new Dictionary<string, DbValue>();

        #region Add Helpers

        public void Add(string name, string value)
            => items.Add(name, new DbValue(value));

        public void Add(string name, Int16 value)
            => items.Add(name, new DbValue(value));

        public void Add(string name, Int32 value)
            => items.Add(name, new DbValue(value));

        public void Add(string name, Int64 value)
            => items.Add(name, new DbValue(value));

        public void Add(string name, float value)
            => items.Add(name, new DbValue(value));

        public void Add(string name, double value)
            => items.Add(name, new DbValue(value));

        public void Add(string name, string[] strings)
            => items.Add(name, new DbValue(strings));

        public void Add(string name, int[] numbers)
            => items.Add(name, new DbValue(numbers));

        public void Add(string name, byte[] data)
            => items.Add(name, new DbValue(data));

        public void Add(string name, DbValue value)
            => items.Add(name, value);

        #endregion

        public int Count => items.Count;

        public bool TryGet(string name, out DbValue value)
            => items.TryGetValue(name, out value);

        public bool ContainsKey(string name)
            => items.ContainsKey(name);

        public DbValue Get(string name)
        {
            DbValue value;

            items.TryGetValue(name, out value);

            return value;
        }

        public HashSet<string> GetStringSet(string key)
        {
            DbValue value;

            if (TryGet(key, out value))
            {
                return value.ToStringSet();
            }

            return null;
        }

        public byte[] GetBinary(string key)
        {
            DbValue value;

            if (TryGet(key, out value))
            {
                return value.ToBinary();
            }

            return null;
        }

        public int GetInt(string key)
        {
            DbValue value;

            if (TryGet(key, out value))
            {
                return value.ToInt();
            }

            return 0;
        }

        public string GetString(string key)
        {
            DbValue value;

            if (TryGet(key, out value))
            {
                return value.ToString();
            }

            return null;
        }

        public void Set(string name, DbValue value)
        {
            items[name] = value;
        }

        public JsonObject ToJson()
        {
            var node = new JsonObject();

            foreach (var attribute in items)
            {
                node.Add(attribute.Key, attribute.Value.ToJson());
            }

            return node;
        }

        // { "hitCount":{"N":"225"}, "date":{"S":"2011-05-31T00:00:00Z"}, "siteId":{"N":"221051"} }

        public static AttributeCollection FromJson(JsonObject json)
        {
            var item = new AttributeCollection();

            foreach (var property in json)
            {
                item.Add(property.Key, DbValue.FromJson(property.Value as JsonObject));
            }

            return item;
        }

        public DbValue this[string key]
        {
            get { return Get(key); }
            set { Set(key, value); }
        }

        #region IEnumerable<KeyValuePair<string,DbValue>> Members

        IEnumerator<KeyValuePair<string, DbValue>> IEnumerable<KeyValuePair<string, DbValue>>.GetEnumerator()
            => items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => items.GetEnumerator();

        #endregion

        #region Helpers

        public static AttributeCollection FromObject(object instance)
            => FromObject(instance, DatasetInfo.Get(instance.GetType()));

        internal static AttributeCollection FromObject(object instance, DatasetInfo schema)
        {
            #region Preconditions

            if (instance == null) throw new ArgumentNullException(nameof(instance));

            #endregion

            var attributes = new AttributeCollection();

            foreach (var member in schema.Members)
            {
                var value = member.GetValue(instance);

                if (value == null) continue;

                var typeInfo = TypeDetails.Get(member.Type);

                IDbValueConverter converter;

                if (DbValueConverterFactory.TryGet(member.Type, out converter))
                {
                    var dbValue = converter.FromObject(value, member);

                    if (dbValue.Kind == DbValueType.Unknown) continue;

                    attributes.Add(member.Name, dbValue);
                }
                else if (member.Type.IsArray)
                {
                    var a = (Array)value;

                    var list = new List<DbValue>(a.Length);

                    foreach (var item in a)
                    {
                        list.Add(new DbValue(FromObject(item)));
                    }

                    attributes.Add(member.Name, new DbValue(list));
                }
                else if (typeInfo.IsList && typeInfo.ElementType != null)
                {
                    var a = (IList)value;

                    var list = new List<DbValue>(a.Count);

                    IDbValueConverter c;

                    if (DbValueConverterFactory.TryGet(typeInfo.ElementType, out c))
                    {
                        foreach (var item in a)
                        {
                            list.Add(c.FromObject(item, member));
                        }
                    }
                    else
                    {
                        foreach (var item in a)
                        {
                            list.Add(new DbValue(FromObject(item)));
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
                        throw new Exception(value.GetType().Name + "/" + typeInfo.IsList);
                    }
                }
            }

            return attributes;
        }

        public RecordKey ToKey()
        {
            var keyItems = new KeyValuePair<string, object>[items.Count];
            var i = 0;

            foreach(var attribute in items)
            {
                keyItems[i] = new KeyValuePair<string, object>(attribute.Key, attribute.Value.ToPrimitiveValue());

                i++;
            }

            return new RecordKey(keyItems);
        }

        public T As<T>() => As<T>(DatasetInfo.Get<T>());

        internal T As<T>(DatasetInfo model)
        {
            var instance = Activator.CreateInstance<T>();

            foreach (var attribute in items)
            {
                var member = model[attribute.Key];

                if (member == null) continue;

                IDbValueConverter converter;

                if (DbValueConverterFactory.TryGet(member.Type, out converter))
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
                    var list = DeserializeArray(member.Type.GetElementType(), (DbValue[])attribute.Value.Value);

                    member.SetValue(instance, list);  
                }
            }

            return instance;
        }

        private Array DeserializeArray(Type elementType, DbValue[] values)
        {
            var elementModel = DatasetInfo.Get(elementType);

            var list = Array.CreateInstance(elementType, values.Length);

            var i = 0;

            foreach (var value in values)
            {
                if (value.Kind == DbValueType.M)
                {
                    list.SetValue(((AttributeCollection)value.Value).DeserializeMap(elementModel), i);
                }

                i++;
            }

            return list;
        }

        private object DeserializeMap(DatasetInfo model)
        {
            var instance = Activator.CreateInstance(model.Type);

            foreach (var attribute in items)
            {
                var member = model[attribute.Key];

                if (member == null) continue;

                IDbValueConverter converter;

                if (DbValueConverterFactory.TryGet(member.Type, out converter))
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
                    var list = DeserializeArray(member.Type.GetElementType(), (DbValue[])attribute.Value.Value);

                    member.SetValue(instance, list);
                }             
            }

            return instance;
        }

        #endregion
    }
}