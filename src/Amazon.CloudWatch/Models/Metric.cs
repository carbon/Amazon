using System.Collections.Generic;
using System.Xml.Linq;

namespace Amazon.CloudWatch;

public sealed class Metric
{
    public Metric(string ns, string name)
    {
        Namespace = ns;
        MetricName = name;
    }

    public string Namespace { get; }

    public string MetricName { get; }

    public List<Dimension> Dimensions { get; } = new List<Dimension>();

    internal static Metric FromXml(XNamespace ns, XElement el)
    {
        var metric = new Metric(
            ns: el.Element(ns + "Namespace")!.Value,
            name: el.Element(ns + "MetricName")!.Value
        );

        if (el.Element(ns + "Dimensions") is XElement dimensionsEl)
        {
            foreach (var dimensionEl in dimensionsEl.Elements())
            {
                metric.Dimensions.Add(new Dimension(
                    name  : dimensionEl.Element(ns + "Name")!.Value,
                    value : dimensionEl.Element(ns + "Value")!.Value
                ));
            }
        }

        return metric;
    }
}

/*
<member>
    <Namespace>AWS/ELB</Namespace>
    <MetricName>BackendConnectionErrors</MetricName>
    <Dimensions>
        <member>
        <Name>Namespace</Name>
        <Value>AWS</Value>
        </member>
    </Dimensions>
</member>
*/