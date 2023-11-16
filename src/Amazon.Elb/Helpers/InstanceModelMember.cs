using System.Reflection;

namespace Amazon.Elb;

public sealed class InstanceModelMember(string name, PropertyInfo property)
{
    public string Name { get; init; } = name;

    public PropertyInfo Property { get; } = property;

    public object? GetValue(object instance)
    {
        return Property.GetValue(instance);
    }
}