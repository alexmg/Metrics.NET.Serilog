using System;
using Metrics.Reports;
using Serilog.Events;

namespace Metrics.Serilog
{
    /// <summary>
    /// Extension methods for configuring the Serilog report for Metrics.NET.
    /// </summary>
    public static class SerilogExtensions
    {
        /// <summary>
        /// Enable the Serilog report for Metrics.NET.
        /// </summary>
        /// <param name="reports">The metrics reports.</param>
        /// <param name="interval">The interval to write out the report.</param>
        /// <param name="defaultLogEventLevel">The default <see cref="LogEventLevel"/> for all metric types.</param>
        /// <returns>The metrics reports to continue report configuration.</returns>
        public static MetricsReports WithSerilog(this MetricsReports reports,
            TimeSpan interval,
            LogEventLevel defaultLogEventLevel = LogEventLevel.Information)
        {
            var serilogReport = new SerilogReport(new MetricLoggingLevels(defaultLogEventLevel));
            return reports.WithReport(serilogReport, interval);
        }

        /// <summary>
        /// Enable the Serilog report for Metrics.NET.
        /// </summary>
        /// <param name="reports">The metrics reports.</param>
        /// <param name="interval">The interval to write out the report.</param>
        /// <param name="defaultLogEventLevel">The default log event level for all metric types.</param>
        /// <param name="metricLoggingLevels">A function to configure the log event level for specific metric types.</param>
        /// <returns>The metrics reports to continue report configuration.</returns>
        public static MetricsReports WithSerilog(this MetricsReports reports,
            TimeSpan interval,
            LogEventLevel defaultLogEventLevel,
            Func<MetricLoggingLevels, MetricLoggingLevels> metricLoggingLevels)
        {
            var levels = metricLoggingLevels(new MetricLoggingLevels(defaultLogEventLevel));
            var serilogReport = new SerilogReport(levels);
            return reports.WithReport(serilogReport, interval);
        }
    }
}