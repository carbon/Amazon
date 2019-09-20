#nullable disable

using System;
using System.Globalization;
using System.Xml.Linq;

namespace Amazon.CloudWatch
{
    public class DataPoint
    {
        public double? Average { get; set; }

        public double? Maximum { get; set; }

        public double? Minimum { get; set; }

        public double? SampleCount { get; set; }

        public double? Sum { get; set; }

        public DateTime Timestamp { get; set; }

        public string Unit { get; set; }

        internal static DataPoint FromXml(XNamespace ns, XElement el)
        {
            var metric = new DataPoint
            {
                Timestamp = DateTime.Parse(el.Element(ns + "Timestamp").Value, null, DateTimeStyles.AdjustToUniversal)
            };

            foreach (var child in el.Elements())
            {
                switch (child.Name.LocalName)
                {
                    case "Average"      : metric.Average     = double.Parse(child.Value, CultureInfo.InvariantCulture); break;
                    case "Maximum"      : metric.Maximum     = double.Parse(child.Value, CultureInfo.InvariantCulture); break;
                    case "Minimum"      : metric.Minimum     = double.Parse(child.Value, CultureInfo.InvariantCulture); break;
                    case "SampleCount"  : metric.SampleCount = double.Parse(child.Value, CultureInfo.InvariantCulture); break;
                    case "Sum"          : metric.Sum         = double.Parse(child.Value, CultureInfo.InvariantCulture); break;
                    case "Unit"         : metric.Unit        = child.Value;                                             break;
                }
            }

            return metric;
        }
    }
}
