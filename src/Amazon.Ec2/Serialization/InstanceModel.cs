using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.Serialization;

namespace Amazon.Ec2.Serialization;

internal sealed class InstanceModel(List<InstanceModelMember> members)
{
    private static readonly ConcurrentDictionary<Type, InstanceModel> s_models = new();

    public List<InstanceModelMember> Members { get; } = members;

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
            if (!property.CanRead) continue;

            if (property.GetCustomAttribute<IgnoreDataMemberAttribute>() != null)
            {
                continue;
            }

            var dataMember = property.GetCustomAttribute<DataMemberAttribute>();
       
            members.Add(new InstanceModelMember(dataMember?.Name ?? property.Name, MethodInvoker.Create(property.GetGetMethod()!)));
        }

        result = new InstanceModel(members);

        s_models.TryAdd(type, result);

        return result;
    }
}