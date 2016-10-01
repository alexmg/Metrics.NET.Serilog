using Serilog.Events;

namespace Metrics.Serilog
{
    /// <summary>
    /// Serilog log event level for different metric types.
    /// </summary>
    public class MetricLoggingLevels : IHideObjectMembers
    {
        internal LogEventLevel GaugeLogEventLevel { get; private set; }
        internal LogEventLevel CounterLogEventLevel { get; private set; }
        internal LogEventLevel MeterLogEventLevel { get; private set; }
        internal LogEventLevel HistogramLogEventLevel { get; private set; }
        internal LogEventLevel TimerLogEventLevel { get; private set; }
        internal LogEventLevel HealthStatusLogEventLevel { get; private set; }

        internal MetricLoggingLevels(LogEventLevel defaultLogEventLevel)
        {
            GaugeLogEventLevel = defaultLogEventLevel;
            CounterLogEventLevel = defaultLogEventLevel;
            MeterLogEventLevel = defaultLogEventLevel;
            HistogramLogEventLevel = defaultLogEventLevel;
            TimerLogEventLevel = defaultLogEventLevel;
            HealthStatusLogEventLevel = defaultLogEventLevel;
        }

        /// <summary>
        /// Configure gauges to be logged at the specified level.
        /// </summary>
        /// <param name="logEventLevel">The log event level.</param>
        /// <returns>A <see cref="MeterLogEventLevel"/> allowing configuration to continue.</returns>
        public MetricLoggingLevels GaugesAt(LogEventLevel logEventLevel)
        {
            GaugeLogEventLevel = logEventLevel;
            return this;
        }

        /// <summary>
        /// Configure counters to be logged at the specified level.
        /// </summary>
        /// <param name="logEventLevel">The log event level.</param>
        /// <returns>A <see cref="MeterLogEventLevel"/> allowing configuration to continue.</returns>
        public MetricLoggingLevels CountersAt(LogEventLevel logEventLevel)
        {
            CounterLogEventLevel = logEventLevel;
            return this;
        }

        /// <summary>
        /// Configure meters to be logged at the specified level.
        /// </summary>
        /// <param name="logEventLevel">The log event level.</param>
        /// <returns>A <see cref="MeterLogEventLevel"/> allowing configuration to continue.</returns>
        public MetricLoggingLevels MetersAt(LogEventLevel logEventLevel)
        {
            MeterLogEventLevel = logEventLevel;
            return this;
        }

        /// <summary>
        /// Configure histograms to be logged at the specified level.
        /// </summary>
        /// <param name="logEventLevel">The log event level.</param>
        /// <returns>A <see cref="MeterLogEventLevel"/> allowing configuration to continue.</returns>
        public MetricLoggingLevels HistogramsAt(LogEventLevel logEventLevel)
        {
            HistogramLogEventLevel = logEventLevel;
            return this;
        }

        /// <summary>
        /// Configure timers to be logged at the specified level.
        /// </summary>
        /// <param name="logEventLevel">The log event level.</param>
        /// <returns>A <see cref="MeterLogEventLevel"/> allowing configuration to continue.</returns>
        public MetricLoggingLevels TimersAt(LogEventLevel logEventLevel)
        {
            TimerLogEventLevel = logEventLevel;
            return this;
        }

        /// <summary>
        /// Configure health status to be logged at the specified level.
        /// </summary>
        /// <param name="logEventLevel">The log event level.</param>
        /// <returns>A <see cref="MeterLogEventLevel"/> allowing configuration to continue.</returns>
        public MetricLoggingLevels HealthStatusAt(LogEventLevel logEventLevel)
        {
            HealthStatusLogEventLevel = logEventLevel;
            return this;
        }
    }
}
