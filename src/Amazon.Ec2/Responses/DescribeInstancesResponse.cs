using System.Xml.Linq;

namespace Amazon.Ec2;

public sealed class DescribeInstancesResponse(List<Instance> instances) : IEc2Response
{
    public IReadOnlyList<Instance> Instances { get; } = instances;

    public static DescribeInstancesResponse Deserialize(string text)
    {
        var rootEl = XElement.Parse(text);

        var ns = rootEl.Name.Namespace;

        var reservationSet = rootEl.Element(ns + "reservationSet")!;

        var instances = new List<Instance>();

        foreach (var itemEl in reservationSet.Elements())
        {
            var instanceSetEl = itemEl.Element(ns + "instancesSet")!;

            foreach (var instanceItemEl in instanceSetEl.Elements())
            {
                instances.Add(Instance.Deserialize(instanceItemEl));
            }
        }

        return new DescribeInstancesResponse(instances);
    }
}