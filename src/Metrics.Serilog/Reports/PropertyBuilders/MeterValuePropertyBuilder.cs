using System.Collections.Generic;
using Metrics.MetricData;
using Serilog.Events;

namespace Metrics.Serilog.Reports.PropertyBuilders
{
    internal static class MeterValuePropertyBuilder
    {
        internal static IEnumerable<LogEventProperty> BuildProperties(this MeterValue value)
        {
            yield return new LogEventProperty(nameof(value.Count), new ScalarValue(value.Count));
            yield return new LogEventProperty(nameof(value.FifteenMinuteRate), new ScalarValue(value.FifteenMinuteRate));
            yield return new LogEventProperty(nameof(value.FiveMinuteRate), new ScalarValue(value.FiveMinuteRate));
            yield return new LogEventProperty(nameof(value.MeanRate), new ScalarValue(value.MeanRate));
            yield return new LogEventProperty(nameof(value.OneMinuteRate), new ScalarValue(value.OneMinuteRate));
            yield return new LogEventProperty(nameof(value.RateUnit), new ScalarValue(value.RateUnit));

            if (value.Items.Length == 0) yield break;

            var itemProperties = value.Items.BuildProperties();
            yield return new LogEventProperty(nameof(value.Items), new StructureValue(itemProperties));
        }
    }
}
