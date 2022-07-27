using System.Xml.Linq;

namespace Amazon.CloudWatch;

public sealed class Metric
{
    public Metric(string ns, string name, List<Dimension> dimensions)
    {
        Namespace = ns;
        MetricName = name;
        Dimensions = dimensions;
    }

    public string Namespace { get; }

    public string MetricName { get; }

    public List<Dimension> Dimensions { get; }

    internal static Metric FromXml(XNamespace ns, XElement el)
    {
        var dimensions = new List<Dimension>();

        if (el.Element(ns + "Dimensions") is XElement dimensionsEl)
        {
            foreach (var dimensionEl in dimensionsEl.Elements())
            {
                dimensions.Add(new Dimension(
                    name  : dimensionEl.Element(ns + "Name")!.Value,
                    value : dimensionEl.Element(ns + "Value")!.Value
                ));
            }
        }

        return new Metric(
            ns         : el.Element(ns + "Namespace")!.Value,
            name       : el.Element(ns + "MetricName")!.Value,
            dimensions : dimensions
        );
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