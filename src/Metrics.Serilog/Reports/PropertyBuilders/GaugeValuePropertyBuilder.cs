using System.Collections.Generic;
using Metrics.MetricData;
using Serilog.Events;

namespace Metrics.Serilog.Reports.PropertyBuilders
{
    internal static class GaugeValuePropertyBuilder
    {
        internal static IEnumerable<LogEventProperty> BuildProperties(this double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value)) yield break;

            yield return new LogEventProperty(PropertyName.Value, new ScalarValue(value));
        }
    }
}
