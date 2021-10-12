using System.Reflection;

namespace Amazon.Ec2;

public sealed class InstanceModelMember
{
    public InstanceModelMember(string name, PropertyInfo property)
    {
        Name = name;
        Property = property;
    }

    public string Name { get; set; }

    public PropertyInfo Property { get; }

    public object GetValue(object instance) => Property.GetValue(instance)!;
}
