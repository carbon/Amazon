using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.Serialization;

namespace Amazon.Elb;

internal sealed class InstanceModel(List<InstanceModelMember> members)
{
    private static readonly ConcurrentDictionary<Type, InstanceModel> s_models = new();

    private readonly List<InstanceModelMember> _members = members;

    public IReadOnlyList<InstanceModelMember> Members => _members;

    public static InstanceModel Get(Type type)
    {
        if (s_models.TryGetValue(type, out var result))
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

        s_models.TryAdd(type, result);

        return result;
    }
}