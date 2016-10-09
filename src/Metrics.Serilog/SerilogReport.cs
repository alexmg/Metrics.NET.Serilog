using System;
using System.Collections.Generic;
using System.Linq;
using Metrics.MetricData;
using Metrics.Serilog.Reports;
using Metrics.Reporters;
using Serilog;
using Serilog.Events;
using Serilog.Parsing;

namespace Metrics.Serilog
{
    internal class SerilogReport : BaseReport
    {
        private readonly LogEventLevel _gaugeLogEventLevel;
        private readonly LogEventLevel _counterLogEventLevel;
        private readonly LogEventLevel _meterLogEventLevel;
        private readonly LogEventLevel _histogramLogEventLevel;
        private readonly LogEventLevel _timerLogEventLevel;
        private readonly LogEventLevel _healthStatusLogEventLevel;

        private readonly MessageTemplateParser _messageTemplateParser = new MessageTemplateParser();

        internal SerilogReport(MetricLoggingLevels metricLoggingLevels)
        {
            _gaugeLogEventLevel = metricLoggingLevels.GaugeLogEventLevel;
            _counterLogEventLevel = metricLoggingLevels.CounterLogEventLevel;
            _meterLogEventLevel = metricLoggingLevels.MeterLogEventLevel;
            _histogramLogEventLevel = metricLoggingLevels.HistogramLogEventLevel;
            _timerLogEventLevel = metricLoggingLevels.TimerLogEventLevel;
            _healthStatusLogEventLevel = metricLoggingLevels.HealthStatusLogEventLevel;
        }

        protected override void ReportGauge(string name, double value, Unit unit, MetricTags tags)
        {
            if (!Log.IsEnabled(_gaugeLogEventLevel)) return;

            var properties = GaugeReport.Build(value, unit, tags);

            WriteLogEvent(name, _gaugeLogEventLevel, properties);
        }

        protected override void ReportCounter(string name, CounterValue value, Unit unit, MetricTags tags)
        {
            if (!Log.IsEnabled(_counterLogEventLevel)) return;

            var properties = CounterReport.Build(value, unit, tags);

            WriteLogEvent(name, _counterLogEventLevel, properties);
        }

        protected override void ReportMeter(string name, MeterValue value, Unit unit, TimeUnit rateUnit, MetricTags tags)
        {
            if (!Log.IsEnabled(_meterLogEventLevel)) return;

            var properties = MeterReport.Build(value, unit, rateUnit, tags);

            WriteLogEvent(name, _meterLogEventLevel, properties);
        }

        protected override void ReportHistogram(string name, HistogramValue value, Unit unit, MetricTags tags)
        {
            if (!Log.IsEnabled(_histogramLogEventLevel)) return;

            var properties = HistogramReport.Build(value, unit, tags);

            WriteLogEvent(name, _histogramLogEventLevel, properties);
        }

        protected override void ReportTimer(string name, TimerValue value, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
        {
            if (!Log.IsEnabled(_timerLogEventLevel)) return;

            var properties = TimerReport.Build(value, unit, rateUnit, durationUnit, tags);

            WriteLogEvent(name, _timerLogEventLevel, properties);
        }

        protected override void ReportHealth(HealthStatus status)
        {
            if (!Log.IsEnabled(_healthStatusLogEventLevel)) return;

            var properties = HealthStatusReport.Build(status);

            WriteLogEvent(nameof(HealthStatus), _healthStatusLogEventLevel, properties);
        }

        private void WriteLogEvent(string name, LogEventLevel logEventLevel, IEnumerable<LogEventProperty> properties)
        {
            var metricProperty = new LogEventProperty(PropertyName.Metric, new ScalarValue(name));
            properties = properties.Concat(new[] {metricProperty});

            var timestamp = new DateTimeOffset(CurrentContextTimestamp);
            var messageTemplate = _messageTemplateParser.Parse(name);
            var logEvent = new LogEvent(timestamp, logEventLevel, null, messageTemplate, properties);

            Log.Write(logEvent);
        }
    }
}
