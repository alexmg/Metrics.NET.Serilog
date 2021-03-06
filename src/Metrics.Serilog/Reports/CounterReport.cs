﻿using System.Collections.Generic;
using System.Linq;
using Metrics.MetricData;
using Metrics.Serilog.Reports.PropertyBuilders;
using Serilog.Events;

namespace Metrics.Serilog.Reports
{
    internal static class CounterReport
    {
        internal static IEnumerable<LogEventProperty> Build(CounterValue value, Unit unit, MetricTags tags)
        {
            var properties = value.BuildProperties()
                .Concat(unit.BuildProperties())
                .Concat(tags.BuildProperties());

            return properties;
        }
    }
}
