using System.Collections.Generic;
using Metrics.MetricData;
using Serilog.Events;

namespace Metrics.Serilog.Reports.PropertyBuilders
{
    internal static class TimerValuePropertyBuilder
    {
        internal static IEnumerable<LogEventProperty> BuildProperties(this TimerValue value)
        {
            yield return new LogEventProperty(nameof(value.ActiveSessions), new ScalarValue(value.ActiveSessions));
            yield return new LogEventProperty(nameof(value.Histogram), new StructureValue(value.Histogram.BuildProperties()));
            yield return new LogEventProperty(nameof(value.Rate), new StructureValue(value.Rate.BuildProperties()));
            yield return new LogEventProperty(nameof(value.TotalTime), new ScalarValue(value.TotalTime));
        }
    }
}
