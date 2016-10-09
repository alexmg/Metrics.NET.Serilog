using System.Collections.Generic;
using Metrics.MetricData;
using Serilog.Events;

namespace Metrics.Serilog.Reports.PropertyBuilders
{
    internal static class SetItemPropertyBuilder
    {
        internal static IEnumerable<LogEventProperty> BuildProperties(this MeterValue.SetItem value)
        {
            foreach (var property in value.Value.BuildProperties())
            {
                yield return property;
            }
            yield return new LogEventProperty(PropertyName.Percent, new ScalarValue(value.Percent));
        }
    }
}
