using System.Collections.Generic;
using Metrics.Serilog.Reports.PropertyBuilders;
using Serilog.Events;

namespace Metrics.Serilog.Reports
{
    internal static class HealthStatusReport
    {
        internal static IEnumerable<LogEventProperty> Build(HealthStatus status)
        {
            var properties = status.BuildProperties();
            return properties;
        }
    }
}
