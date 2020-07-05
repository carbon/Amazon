using Amazon.DynamoDb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json;

namespace Amazon.DynamoDb.Extensions
{
    public static class JsonElementExtensions
    {
        public static string[] GetStringArray(this JsonElement element)
        {
            string[] items = new string[element.GetArrayLength()];

            int i = 0;

            foreach (var el in element.EnumerateArray())
            {
                items[i] = el.GetString();

                i++;
            }

            return items;
        }

        public static T GetEnum<T>(this JsonElement element) where T : struct, Enum
        {
            if (Enum.TryParse<T>(element.GetString(), out T enumValue))
            {
                return enumValue;
            }

            throw new InvalidEnumArgumentException($"{element.GetString()} could not be parsed as enum {typeof(T)}");
        }

        public static DateTimeOffset GetDynamoDateTimeOffset(this JsonElement element)
        {
            long timestampSeconds = element.GetInt64();

            return DateTimeOffset.FromUnixTimeSeconds(timestampSeconds);
        }

        public static T GetObject<T>(this JsonElement el) where T : IConvertibleFromJson, new()
        {
            T obj = new T();
            foreach (var prop in el.EnumerateObject())
            {
                obj.FillField(prop);
            }
            return obj;
        }

        public static T[] GetObjectArray<T>(this JsonElement el) where T : IConvertibleFromJson, new()
        {
            T[] objArray = new T[el.GetArrayLength()];

            int i = 0;
            foreach (var childElement in el.EnumerateArray())
            {
                objArray[i++] = GetObject<T>(childElement);
            }

            return objArray;
        }
    }
}
