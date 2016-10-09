using System.Linq;
using FluentAssertions;
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

            report[0].Name.Should().Be(PropertyName.Count);
            report[1].Name.Should().Be(PropertyName.Items);
            report[2].Name.Should().Be(PropertyName.Unit);
            report[3].Name.Should().Be(PropertyName.Tags);
        }

        [Test]
        public void BuildGaugeReport()
        {
            var value = Test.Gauge.Value;

            var report = GaugeReport.Build(value, Unit.Bytes, Test.Tags.ThreeTags).ToList();

            report.Should().HaveCount(3);

            report[0].Name.Should().Be(PropertyName.Value);
            report[1].Name.Should().Be(PropertyName.Unit);
            report[2].Name.Should().Be(PropertyName.Tags);
        }

        [Test]
        public void BuildHealthStatusReport()
        {
            var value = Test.HealthStatus.WithTwoResults;

            var report = HealthStatusReport.Build(value).ToList();

            report.Should().HaveCount(4);

            report[0].Name.Should().Be(PropertyName.HasRegisteredChecks);
            report[1].Name.Should().Be(PropertyName.IsHealthy);
            report[2].Name.Should().Be(value.Results[0].Name);
            report[3].Name.Should().Be(value.Results[1].Name);
        }

        [Test]
        public void BuildHistogramReport()
        {
            var value = Test.Histogram.Value;

            var report = HistogramReport.Build(value, Unit.Bytes, Test.Tags.ThreeTags).ToList();

            report.Should().HaveCount(18);

            report[0].Name.Should().Be(PropertyName.Count);
            report[1].Name.Should().Be(PropertyName.LastUserValue);
            report[2].Name.Should().Be(PropertyName.LastValue);
            report[3].Name.Should().Be(PropertyName.Max);
            report[4].Name.Should().Be(PropertyName.MaxUserValue);
            report[5].Name.Should().Be(PropertyName.Mean);
            report[6].Name.Should().Be(PropertyName.Median);
            report[7].Name.Should().Be(PropertyName.Min);
            report[8].Name.Should().Be(PropertyName.MinUserValue);
            report[9].Name.Should().Be(PropertyName.Percentile75);
            report[10].Name.Should().Be(PropertyName.Percentile95);
            report[11].Name.Should().Be(PropertyName.Percentile98);
            report[12].Name.Should().Be(PropertyName.Percentile99);
            report[13].Name.Should().Be(PropertyName.Percentile999);
            report[14].Name.Should().Be(PropertyName.SampleSize);
            report[15].Name.Should().Be(PropertyName.StdDev);
            report[16].Name.Should().Be(PropertyName.Unit);
            report[17].Name.Should().Be(PropertyName.Tags);
        }

        [Test]
        public void BuildMeterReport()
        {
            var value = Test.Meter.WithTwoItems;

            var report = MeterReport.Build(value, Unit.Bytes, TimeUnit.Seconds, Test.Tags.ThreeTags).ToList();

            report.Should().HaveCount(10);

            report[0].Name.Should().Be(PropertyName.Count);
            report[1].Name.Should().Be(PropertyName.FifteenMinuteRate);
            report[2].Name.Should().Be(PropertyName.FiveMinuteRate);
            report[3].Name.Should().Be(PropertyName.MeanRate);
            report[4].Name.Should().Be(PropertyName.OneMinuteRate);
            report[5].Name.Should().Be(PropertyName.RateUnit);
            report[6].Name.Should().Be(PropertyName.Items);
            report[7].Name.Should().Be(PropertyName.Unit);
            report[8].Name.Should().Be(PropertyName.RateUnit);
            report[9].Name.Should().Be(PropertyName.Tags);
        }

        [Test]
        public void BuildTimerReport()
        {
            var value = Test.Timer.Value;

            var report = TimerReport.Build(value, Unit.Bytes, TimeUnit.Seconds, TimeUnit.Seconds, Test.Tags.ThreeTags).ToList();

            report.Should().HaveCount(8);

            report[0].Name.Should().Be(PropertyName.ActiveSessions);
            report[1].Name.Should().Be(PropertyName.Histogram);
            report[2].Name.Should().Be(PropertyName.Rate);
            report[3].Name.Should().Be(PropertyName.TotalTime);
            report[4].Name.Should().Be(PropertyName.Unit);
            report[5].Name.Should().Be(PropertyName.RateUnit);
            report[6].Name.Should().Be(PropertyName.DurationUnit);
            report[7].Name.Should().Be(PropertyName.Tags);
        }
    }
}
