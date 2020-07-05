using System;
using System.Collections.Generic;
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
    }
}
