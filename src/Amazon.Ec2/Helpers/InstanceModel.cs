﻿using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.Serialization;

namespace Amazon.Ec2;

internal sealed class InstanceModel
{
    private static readonly ConcurrentDictionary<Type, InstanceModel> models = new();

    public InstanceModel(IReadOnlyList<InstanceModelMember> members)
    {
        Members = members;
    }

    public IReadOnlyList<InstanceModelMember> Members { get; }

    public static InstanceModel Get(Type type)
    {
        if (models.TryGetValue(type, out var result))
        {
            return result;
        }

        var properties = type.GetProperties();

        var members = new List<InstanceModelMember>(properties.Length);

        foreach (var property in properties)
        {
            if (property.GetCustomAttribute<IgnoreDataMemberAttribute>() != null)
            {
                continue;
            }

            var dataMember = property.GetCustomAttribute<DataMemberAttribute>();

            members.Add(new InstanceModelMember(dataMember?.Name ?? property.Name, property));
        }

        result = new InstanceModel(members);

        models.TryAdd(type, result);

        return result;
    }
}