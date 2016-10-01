using System.Collections.Generic;
using Serilog.Events;

namespace Metrics.Serilog.Reports.PropertyBuilders
{
    internal static class TimeUnitPropertyBuilder
    {
        internal static IEnumerable<LogEventProperty> BuildProperties(this TimeUnit value, string name)
        {
            yield return new LogEventProperty(name, new ScalarValue(value));
        }
    }
}
