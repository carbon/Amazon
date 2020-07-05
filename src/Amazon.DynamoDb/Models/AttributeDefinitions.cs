using System;
using System.Collections;
using System.Collections.Generic;

using Carbon.Json;
using Carbon.Data;
using System.Linq;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Amazon.DynamoDb
{
    public sealed class AttributeDefinitions : IEnumerable<KeyValuePair<string, DbValueType>>
    {
        private static readonly DbValueType[] ALLOWED_KINDS =
        {
            DbValueType.S,
            DbValueType.N,
            DbValueType.B,
        };

        private readonly Dictionary<string, DbValueType> items;

        public AttributeDefinitions(int capacity)
        {
            items = new Dictionary<string, DbValueType>(capacity);
        }

        public AttributeDefinitions()
        {
            items = new Dictionary<string, DbValueType>();
        }

        #region Add Helpers

        public void Add(string name, DbValueType kind)
            => items.Add(name, kind);

        #endregion

        public int Count => items.Count;

        public bool TryGet(string name, out DbValueType kind) => 
            items.TryGetValue(name, out kind);

        public bool ContainsKey(string name) => items.ContainsKey(name);

        public DbValueType Get(string name)
        {
            items.TryGetValue(name, out DbValueType kind);

            return kind;
        }

        public void Set(string name, DbValueType kind)
        {
            if (!ALLOWED_KINDS.Contains(kind))
            {
                throw new ArgumentException($"{kind} is not a valid attribute type. Allowed types are {String.Join(", ", ALLOWED_KINDS)}");
            }

            items[name] = kind;
        }

        public JsonObject ToJson()
        {
            var node = new JsonObject(capacity: items.Count);

            foreach (var attributeKind in items)
            {
                node.Add(attributeKind.Key, attributeKind.Value.ToQuickString());
            }

            return node;
        }

        // { "hitCount":{"N":"225"}, "date":{"S":"2011-05-31T00:00:00Z"}, "siteId":{"N":"221051"} }

        public static AttributeDefinitions FromJson(JsonObject json)
        {
            var item = new AttributeDefinitions(json.Keys.Count);

            foreach (var property in json)
            {
                Enum.TryParse(property.Value.ToString(), out DbValueType dbValueType);
                item.Add(property.Key, dbValueType);
            }

            return item;
        }

        public static AttributeDefinitions FromJsonElement(JsonElement json)
        {
            var item = new AttributeDefinitions();

            foreach (var property in json.EnumerateObject())
            {
                if (Enum.TryParse<DbValueType>(property.Value.GetString(), out DbValueType kind))
                {
                    item.Add(property.Name, kind);
                }
            }

            return item;
        }

        public DbValueType this[string key]
        {
            get => Get(key);
            set => Set(key, value);
        }

        #region IEnumerable<KeyValuePair<string,DbValueType>> Members

        IEnumerator<KeyValuePair<string, DbValueType>> IEnumerable<KeyValuePair<string, DbValueType>>.GetEnumerator()
            => items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => items.GetEnumerator();

        #endregion
    }
}