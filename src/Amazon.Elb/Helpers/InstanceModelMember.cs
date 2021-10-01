using System.Reflection;

namespace Amazon.Elb;

public sealed class InstanceModelMember
{
    public InstanceModelMember(string name, PropertyInfo property)
    {
        Name = name;
        Property = property;
    }

    public string Name { get; init; }

    public PropertyInfo Property { get; }

    public object? GetValue(object instance)
    {
        return Property.GetValue(instance);
    }
}