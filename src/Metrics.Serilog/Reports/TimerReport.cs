using System.Collections.Generic;
using System.Linq;
using Metrics.MetricData;
using Metrics.Serilog.Reports.PropertyBuilders;
using Serilog.Events;

namespace Metrics.Serilog.Reports
{
    internal static class TimerReport
    {
        internal static IEnumerable<LogEventProperty> Build(TimerValue value, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
        {
            var properties = value.BuildProperties()
                .Concat(unit.BuildProperties())
                .Concat(rateUnit.BuildProperties(nameof(TimerValueSource.RateUnit)))
                .Concat(durationUnit.BuildProperties(nameof(TimerValueSource.DurationUnit)))
                .Concat(tags.BuildProperties());

            return properties;
        }
    }
}
