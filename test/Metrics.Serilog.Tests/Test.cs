using System.Linq;
using Metrics.Core;
using Metrics.MetricData;

namespace Metrics.Serilog.Tests
{
    internal static class Test
    {
        internal static class Counter
        {
            private static readonly CounterValue.SetItem Item1 = new CounterValue.SetItem("Item1", 150, 75);

            private static readonly CounterValue.SetItem Item2 = new CounterValue.SetItem("Item2", 50, 25);

            internal static CounterValue WithTwoItems = new CounterValue(200, new[] {Item1, Item2});
        }

        internal static class Gauge
        {
            internal const double Value = 123.456D;

            internal const double NaN = double.NaN;

            internal const double PositiveInfinity = double.PositiveInfinity;

            internal const double NegativeInfinity = double.NegativeInfinity;
        }

        internal static class HealthStatus
        {
            private static readonly HealthCheck.Result HealthyResult = new HealthCheck.Result("Result1", HealthCheckResult.Healthy());

            private static readonly HealthCheck.Result UnhealthyResult = new HealthCheck.Result("Result2", HealthCheckResult.Unhealthy());

            internal static readonly Metrics.HealthStatus WithTwoResults = new Metrics.HealthStatus(new[] {HealthyResult, UnhealthyResult});

            internal static readonly Metrics.HealthStatus WithNoResults = new Metrics.HealthStatus(Enumerable.Empty<HealthCheck.Result>());
        }

        internal static class Histogram
        {
            internal static readonly HistogramValue Value = new HistogramValue(1, 2, "LastUserValue", 3, "MaxUserValue", 4, 5, "MinUserValue", 6, 7, 8, 9, 10, 11, 12, 13);
        }

        internal static class Meter
        {
            private static readonly MeterValue.SetItem Item1 = new MeterValue.SetItem("Item1", 0.5,
                new MeterValue(1, 2, 3, 4, 5, TimeUnit.Seconds, new MeterValue.SetItem[0]));

            private static readonly  MeterValue.SetItem Item2 = new MeterValue.SetItem("Item2", 0.5,
                new MeterValue(1, 2, 3, 4, 5, TimeUnit.Seconds, new MeterValue.SetItem[0]));

            internal static readonly MeterValue WithTwoItems = new MeterValue(5, 1, 2, 3, 4, TimeUnit.Seconds, new[] {Item1, Item2});

            internal static readonly MeterValue WithNoItems = new MeterValue(5, 1, 2, 3, 4, TimeUnit.Seconds, new MeterValue.SetItem[0]);
        }

        internal static class Timer
        {
            internal static readonly TimerValue Value = new TimerValue(Meter.WithNoItems, Histogram.Value, 0, 1, TimeUnit.Seconds);
        }

        public static class Tags
        {
            internal static readonly MetricTags ThreeTags = new MetricTags("A", "B", "C");

            internal static readonly MetricTags Empty = MetricTags.None;
        }
    }
}
