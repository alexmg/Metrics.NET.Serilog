using FluentAssertions;
using NUnit.Framework;
using Serilog.Events;

namespace Metrics.Serilog.Tests
{
    internal sealed class MetricLoggingLevelsTests
    {
        [Test]
        public void DefaultLogEventLevelAppliesToAllMetricTypes()
        {
            const LogEventLevel defaultLevel = LogEventLevel.Debug;

            var levels = new MetricLoggingLevels(defaultLevel);

            levels.CounterLogEventLevel.Should().Be(defaultLevel);
            levels.GaugeLogEventLevel.Should().Be(defaultLevel);
            levels.HealthStatusLogEventLevel.Should().Be(defaultLevel);
            levels.HistogramLogEventLevel.Should().Be(defaultLevel);
            levels.MeterLogEventLevel.Should().Be(defaultLevel);
            levels.TimerLogEventLevel.Should().Be(defaultLevel);
        }

        [Test]
        public void CountersAtOverridesDefaultLogEventLevelLogEventLevel()
        {
            var levels = new MetricLoggingLevels(LogEventLevel.Information);

            levels.CountersAt(LogEventLevel.Debug);

            levels.CounterLogEventLevel.Should().Be(LogEventLevel.Debug);
        }

        [Test]
        public void GaugesAtOverridesDefaultLogEventLevel()
        {
            var levels = new MetricLoggingLevels(LogEventLevel.Information);

            levels.GaugesAt(LogEventLevel.Debug);

            levels.GaugeLogEventLevel.Should().Be(LogEventLevel.Debug);
        }

        [Test]
        public void HealthStatusAtOverridesDefaultLogEventLevel()
        {
            var levels = new MetricLoggingLevels(LogEventLevel.Information);

            levels.HealthStatusAt(LogEventLevel.Debug);

            levels.HealthStatusLogEventLevel.Should().Be(LogEventLevel.Debug);
        }

        [Test]
        public void HistogramsAtOverridesDefaultLogEventLevel()
        {
            var levels = new MetricLoggingLevels(LogEventLevel.Information);

            levels.HistogramsAt(LogEventLevel.Debug);

            levels.HistogramLogEventLevel.Should().Be(LogEventLevel.Debug);
        }

        [Test]
        public void MetersAtOverridesDefaultLogEventLevel()
        {
            var levels = new MetricLoggingLevels(LogEventLevel.Information);

            levels.MetersAt(LogEventLevel.Debug);

            levels.MeterLogEventLevel.Should().Be(LogEventLevel.Debug);
        }

        [Test]
        public void TimersAtOverridesDefaultLogEventLevel()
        {
            var levels = new MetricLoggingLevels(LogEventLevel.Information);

            levels.TimersAt(LogEventLevel.Debug);

            levels.TimerLogEventLevel.Should().Be(LogEventLevel.Debug);
        }
    }
}