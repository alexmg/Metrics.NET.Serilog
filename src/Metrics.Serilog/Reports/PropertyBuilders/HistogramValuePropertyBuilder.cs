using System.Collections.Generic;
using Metrics.MetricData;
using Serilog.Events;

namespace Metrics.Serilog.Reports.PropertyBuilders
{
    internal static class HistogramValuePropertyBuilder
    {
        internal static IEnumerable<LogEventProperty> BuildProperties(this HistogramValue value)
        {
            yield return new LogEventProperty(nameof(value.Count), new ScalarValue(value.Count));
            yield return new LogEventProperty(nameof(value.LastUserValue), new ScalarValue(value.LastUserValue));
            yield return new LogEventProperty(nameof(value.LastValue), new ScalarValue(value.LastValue));
            yield return new LogEventProperty(nameof(value.Max), new ScalarValue(value.Max));
            yield return new LogEventProperty(nameof(value.MaxUserValue), new ScalarValue(value.MaxUserValue));
            yield return new LogEventProperty(nameof(value.Mean), new ScalarValue(value.Mean));
            yield return new LogEventProperty(nameof(value.Median), new ScalarValue(value.Median));
            yield return new LogEventProperty(nameof(value.Min), new ScalarValue(value.Min));
            yield return new LogEventProperty(nameof(value.MinUserValue), new ScalarValue(value.MinUserValue));
            yield return new LogEventProperty(nameof(value.Percentile75), new ScalarValue(value.Percentile75));
            yield return new LogEventProperty(nameof(value.Percentile95), new ScalarValue(value.Percentile95));
            yield return new LogEventProperty(nameof(value.Percentile98), new ScalarValue(value.Percentile98));
            yield return new LogEventProperty(nameof(value.Percentile99), new ScalarValue(value.Percentile99));
            yield return new LogEventProperty(nameof(value.Percentile999), new ScalarValue(value.Percentile999));
            yield return new LogEventProperty(nameof(value.SampleSize), new ScalarValue(value.SampleSize));
            yield return new LogEventProperty(nameof(value.StdDev), new ScalarValue(value.StdDev));
        }
    }
}
