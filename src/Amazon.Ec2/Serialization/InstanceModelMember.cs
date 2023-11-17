using System.Reflection;

namespace Amazon.Ec2.Serialization;

internal sealed class InstanceModelMember(string name, MethodInvoker getMethodInvoker)
{
    public string Name { get; set; } = name;

    public MethodInvoker GetMethodInvoker { get; } = getMethodInvoker;

    public object GetValue(object instance) => GetMethodInvoker.Invoke(instance)!;
}