using System.Collections.Generic;
using System.Linq;
using Metrics.MetricData;
using Serilog.Events;

namespace Metrics.Serilog.Reports.PropertyBuilders
{
    internal static class CounterValuePropertyBuilder
    {
        internal static IEnumerable<LogEventProperty> BuildProperties(this CounterValue value)
        {
            yield return new LogEventProperty(PropertyName.Count, new ScalarValue(value.Count));

            if (value.Items.Length == 0) yield break;

            var itemProperties = value.Items.Select(item =>
            {
                var countProperty = new LogEventProperty(PropertyName.Count, new ScalarValue(item.Count));
                var percentProperty = new LogEventProperty(PropertyName.Percent, new ScalarValue(item.Percent));
                return new LogEventProperty(item.Item, new StructureValue(new[] {countProperty, percentProperty}));
            });
            yield return new LogEventProperty(PropertyName.Items, new StructureValue(itemProperties));
        }
    }
}
