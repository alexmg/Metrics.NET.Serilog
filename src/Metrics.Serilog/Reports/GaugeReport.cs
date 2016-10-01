using System.Collections.Generic;
using System.Linq;
using Metrics.Serilog.Reports.PropertyBuilders;
using Serilog.Events;

namespace Metrics.Serilog.Reports
{
    internal static class GaugeReport
    {
        internal static IEnumerable<LogEventProperty> Build(double value, Unit unit, MetricTags tags)
        {
            var properties = value.BuildProperties()
                .Concat(unit.BuildProperties())
                .Concat(tags.BuildProperties());

            return properties;
        }
    }
}
