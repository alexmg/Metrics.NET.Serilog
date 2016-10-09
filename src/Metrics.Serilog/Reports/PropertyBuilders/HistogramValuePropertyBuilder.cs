using System.Collections.Generic;
using Metrics.MetricData;
using Serilog.Events;

namespace Metrics.Serilog.Reports.PropertyBuilders
{
    internal static class HistogramValuePropertyBuilder
    {
        internal static IEnumerable<LogEventProperty> BuildProperties(this HistogramValue value)
        {
            yield return new LogEventProperty(PropertyName.Count, new ScalarValue(value.Count));
            yield return new LogEventProperty(PropertyName.LastUserValue, new ScalarValue(value.LastUserValue));
            yield return new LogEventProperty(PropertyName.LastValue, new ScalarValue(value.LastValue));
            yield return new LogEventProperty(PropertyName.Max, new ScalarValue(value.Max));
            yield return new LogEventProperty(PropertyName.MaxUserValue, new ScalarValue(value.MaxUserValue));
            yield return new LogEventProperty(PropertyName.Mean, new ScalarValue(value.Mean));
            yield return new LogEventProperty(PropertyName.Median, new ScalarValue(value.Median));
            yield return new LogEventProperty(PropertyName.Min, new ScalarValue(value.Min));
            yield return new LogEventProperty(PropertyName.MinUserValue, new ScalarValue(value.MinUserValue));
            yield return new LogEventProperty(PropertyName.Percentile75, new ScalarValue(value.Percentile75));
            yield return new LogEventProperty(PropertyName.Percentile95, new ScalarValue(value.Percentile95));
            yield return new LogEventProperty(PropertyName.Percentile98, new ScalarValue(value.Percentile98));
            yield return new LogEventProperty(PropertyName.Percentile99, new ScalarValue(value.Percentile99));
            yield return new LogEventProperty(PropertyName.Percentile999, new ScalarValue(value.Percentile999));
            yield return new LogEventProperty(PropertyName.SampleSize, new ScalarValue(value.SampleSize));
            yield return new LogEventProperty(PropertyName.StdDev, new ScalarValue(value.StdDev));
        }
    }
}
