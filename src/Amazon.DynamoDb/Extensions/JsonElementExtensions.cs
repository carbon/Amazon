﻿using System.Text.Json;

namespace Amazon.DynamoDb.Extensions;

internal static class JsonElementExtensions
{
    public static string[] GetStringArray(this in JsonElement element)
    {
        var items = new string[element.GetArrayLength()];

        int i = 0;

        foreach (var el in element.EnumerateArray())
        {
            items[i] = el.GetString()!;

            i++;
        }

        return items;
    }
}
