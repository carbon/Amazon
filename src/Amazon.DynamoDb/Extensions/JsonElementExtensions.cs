using System;
using System.ComponentModel;
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
            double timestampSeconds = element.GetDouble();

            return DateTimeOffset.FromUnixTimeMilliseconds((long)(timestampSeconds * 1000d));
        }
    }
}
