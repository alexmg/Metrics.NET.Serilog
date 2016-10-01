using System.Collections.Generic;
using System.Linq;
using Metrics.MetricData;
using Metrics.Serilog.Reports.PropertyBuilders;
using Serilog.Events;

namespace Metrics.Serilog.Reports
{
    internal static class MeterReport
    {
        internal static IEnumerable<LogEventProperty> Build(MeterValue value, Unit unit, TimeUnit rateUnit, MetricTags tags)
        {
            var properties = value.BuildProperties()
                .Concat(unit.BuildProperties())
                .Concat(rateUnit.BuildProperties(nameof(MeterValueSource.RateUnit)))
                .Concat(tags.BuildProperties());

            return properties;
        }
    }
}
