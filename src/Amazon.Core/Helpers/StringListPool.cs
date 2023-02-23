// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// Based on: https://github.com/dotnet/runtime/tree/main/src/libraries/System.Reflection.Metadata/src/System/Reflection/Internal/Utilities/ObjectPool%601.cs

using System.Threading;

namespace Amazon.Helpers;

internal sealed class StringListPool
{
    private struct Element
    {
        internal List<string>? Value;
    }

    private readonly Element[] _items; // storage

    internal StringListPool()
        : this(Environment.ProcessorCount * 2)
    { }

    internal StringListPool(int size)
    {
        _items = new Element[size];
    }

    private static List<string> CreateInstance()
    {
        return new List<string>(8);
    }

    internal List<string> Rent()
    {
        var items = _items;

        List<string>? inst;

        for (int i = 0; i < items.Length; i++)
        {
            inst = items[i].Value;

            if (inst != null)
            {
                if (inst == Interlocked.CompareExchange(ref items[i].Value, null, inst))
                {
                    goto gotInstance;
                }
            }
        }

        inst = CreateInstance();

    gotInstance:
        return inst;
    }

    internal void Return(List<string> obj)
    {
        if (obj.Count > 32) return;

        obj.Clear();

        var items = _items;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].Value == null)
            {
                items[i].Value = obj;
                break;
            }
        }
    }
}