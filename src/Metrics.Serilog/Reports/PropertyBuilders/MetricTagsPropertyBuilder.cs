using System.Collections.Generic;
using System.Linq;
using Serilog.Events;

namespace Metrics.Serilog.Reports.PropertyBuilders
{
    internal static class MetricTagsPropertyBuilder
    {
        internal static IEnumerable<LogEventProperty> BuildProperties(this MetricTags value)
        {
            if (value.Tags.Length == 0) yield break;

            var tags = value.Tags.Select(t => new ScalarValue(t));
            var tagProperty = new LogEventProperty(PropertyName.Tags, new SequenceValue(tags));

            yield return tagProperty;
        }
    }
}
