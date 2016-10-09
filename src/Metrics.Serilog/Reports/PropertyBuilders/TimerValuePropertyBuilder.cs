using System.Collections.Generic;
using Metrics.MetricData;
using Serilog.Events;

namespace Metrics.Serilog.Reports.PropertyBuilders
{
    internal static class TimerValuePropertyBuilder
    {
        internal static IEnumerable<LogEventProperty> BuildProperties(this TimerValue value)
        {
            yield return new LogEventProperty(PropertyName.ActiveSessions, new ScalarValue(value.ActiveSessions));
            yield return new LogEventProperty(PropertyName.Histogram, new StructureValue(value.Histogram.BuildProperties()));
            yield return new LogEventProperty(PropertyName.Rate, new StructureValue(value.Rate.BuildProperties()));
            yield return new LogEventProperty(PropertyName.TotalTime, new ScalarValue(value.TotalTime));
        }
    }
}
