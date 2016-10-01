using System.Collections.Generic;
using Serilog.Events;

namespace Metrics.Serilog.Reports.PropertyBuilders
{
    internal static class HealthStatusPropertyBuilder
    {
        internal static IEnumerable<LogEventProperty> BuildProperties(this HealthStatus value)
        {
            yield return new LogEventProperty(nameof(value.HasRegisteredChecks), new ScalarValue(value.HasRegisteredChecks));

            if (!value.HasRegisteredChecks) yield break;

            yield return new LogEventProperty(nameof(value.IsHealthy), new ScalarValue(value.IsHealthy));

            foreach (var result in value.Results)
            {
                var isHealthy = new LogEventProperty(nameof(result.Check.IsHealthy), new ScalarValue(result.Check.IsHealthy));
                var message = new LogEventProperty(nameof(result.Check.Message), new ScalarValue(result.Check.Message));
                yield return new LogEventProperty(result.Name, new StructureValue(new[] {isHealthy, message}));
            }
        }
    }
}
