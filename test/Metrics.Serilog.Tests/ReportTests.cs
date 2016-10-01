using System.Linq;
using FluentAssertions;
using Metrics.MetricData;
using Metrics.Serilog.Reports;
using NUnit.Framework;

namespace Metrics.Serilog.Tests
{
    internal sealed class ReportTests
    {
        [Test]
        public void BuildCounterReport()
        {
            var value = Test.Counter.WithTwoItems;

            var report = CounterReport.Build(value, Unit.Commands, Test.Tags.ThreeTags).ToList();

            report.Should().HaveCount(4);

            report[0].Name.Should().Be(nameof(CounterValue.Count));
            report[1].Name.Should().Be(nameof(value.Items));
            report[2].Name.Should().Be(nameof(Unit));
            report[3].Name.Should().Be(nameof(MetricTags.Tags));
        }

        [Test]
        public void BuildGaugeReport()
        {
            var value = Test.Gauge.Value;

            var report = GaugeReport.Build(value, Unit.Bytes, Test.Tags.ThreeTags).ToList();

            report.Should().HaveCount(3);

            report[0].Name.Should().Be(nameof(GaugeValueSource.Value));
            report[1].Name.Should().Be(nameof(Unit));
            report[2].Name.Should().Be(nameof(MetricTags.Tags));
        }

        [Test]
        public void BuildHealthStatusReport()
        {
            var value = Test.HealthStatus.WithTwoResults;

            var report = HealthStatusReport.Build(value).ToList();

            report.Should().HaveCount(4);

            report[0].Name.Should().Be(nameof(HealthStatus.HasRegisteredChecks));
            report[1].Name.Should().Be(nameof(HealthStatus.IsHealthy));
            report[2].Name.Should().Be(value.Results[0].Name);
            report[3].Name.Should().Be(value.Results[1].Name);
        }

        [Test]
        public void BuildHistogramReport()
        {
            var value = Test.Histogram.Value;

            var report = HistogramReport.Build(value, Unit.Bytes, Test.Tags.ThreeTags).ToList();

            report.Should().HaveCount(18);

            report[0].Name.Should().Be(nameof(value.Count));
            report[1].Name.Should().Be(nameof(value.LastUserValue));
            report[2].Name.Should().Be(nameof(value.LastValue));
            report[3].Name.Should().Be(nameof(value.Max));
            report[4].Name.Should().Be(nameof(value.MaxUserValue));
            report[5].Name.Should().Be(nameof(value.Mean));
            report[6].Name.Should().Be(nameof(value.Median));
            report[7].Name.Should().Be(nameof(value.Min));
            report[8].Name.Should().Be(nameof(value.MinUserValue));
            report[9].Name.Should().Be(nameof(value.Percentile75));
            report[10].Name.Should().Be(nameof(value.Percentile95));
            report[11].Name.Should().Be(nameof(value.Percentile98));
            report[12].Name.Should().Be(nameof(value.Percentile99));
            report[13].Name.Should().Be(nameof(value.Percentile999));
            report[14].Name.Should().Be(nameof(value.SampleSize));
            report[15].Name.Should().Be(nameof(value.StdDev));
            report[16].Name.Should().Be(nameof(Unit));
            report[17].Name.Should().Be(nameof(MetricTags.Tags));
        }

        [Test]
        public void BuildMeterReport()
        {
            var value = Test.Meter.WithTwoItems;

            var report = MeterReport.Build(value, Unit.Bytes, TimeUnit.Seconds, Test.Tags.ThreeTags).ToList();

            report.Should().HaveCount(10);

            report[0].Name.Should().Be(nameof(value.Count));
            report[1].Name.Should().Be(nameof(value.FifteenMinuteRate));
            report[2].Name.Should().Be(nameof(value.FiveMinuteRate));
            report[3].Name.Should().Be(nameof(value.MeanRate));
            report[4].Name.Should().Be(nameof(value.OneMinuteRate));
            report[5].Name.Should().Be(nameof(value.RateUnit));
            report[6].Name.Should().Be(nameof(value.Items));
            report[7].Name.Should().Be(nameof(Unit));
            report[8].Name.Should().Be(nameof(MeterValueSource.RateUnit));
            report[9].Name.Should().Be(nameof(MetricTags.Tags));
        }

        [Test]
        public void BuildTimerReport()
        {
            var value = Test.Timer.Value;

            var report = TimerReport.Build(value, Unit.Bytes, TimeUnit.Seconds, TimeUnit.Seconds, Test.Tags.ThreeTags).ToList();

            report.Should().HaveCount(8);

            report[0].Name.Should().Be(nameof(value.ActiveSessions));
            report[1].Name.Should().Be(nameof(value.Histogram));
            report[2].Name.Should().Be(nameof(value.Rate));
            report[3].Name.Should().Be(nameof(value.TotalTime));
            report[4].Name.Should().Be(nameof(Unit));
            report[5].Name.Should().Be(nameof(TimerValueSource.RateUnit));
            report[6].Name.Should().Be(nameof(TimerValueSource.DurationUnit));
            report[7].Name.Should().Be(nameof(MetricTags.Tags));
        }
    }
}
