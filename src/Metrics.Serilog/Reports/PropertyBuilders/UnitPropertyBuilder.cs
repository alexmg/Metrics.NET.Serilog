using System.Collections.Generic;
using Serilog.Events;

namespace Metrics.Serilog.Reports.PropertyBuilders
{
    internal static class UnitPropertyBuilder
    {
        internal static IEnumerable<LogEventProperty> BuildProperties(this Unit value)
        {
            yield return new LogEventProperty(PropertyName.Unit, new ScalarValue(value.Name));
        }
    }
}
