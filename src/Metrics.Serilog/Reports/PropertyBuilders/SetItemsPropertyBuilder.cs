using System.Collections.Generic;
using System.Linq;
using Metrics.MetricData;
using Serilog.Events;

namespace Metrics.Serilog.Reports.PropertyBuilders
{
    internal static class SetItemsPropertyBuilder
    {
        internal static IEnumerable<LogEventProperty> BuildProperties(this IEnumerable<MeterValue.SetItem> values)
        {
            return values.Select(item => new LogEventProperty(item.Item, new StructureValue(item.BuildProperties())));
        }
    }
}
