using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Metrics.Core;
using Metrics.MetricData;
using Metrics.Serilog.Reports.PropertyBuilders;
using NUnit.Framework;
using Serilog.Events;

namespace Metrics.Serilog.Tests
{
    internal sealed class PropertyBuilderTests
    {
        [Test]
        public void CounterValuePropertyBuilder()
        {
            var value = Test.Counter.WithTwoItems;

            var properties = value.BuildProperties().ToList();

            properties.Should().HaveCount(2);

            properties[0].Name.Should().Be(PropertyName.Count);
            properties[0].Value.As<ScalarValue>().Value.Should().Be(value.Count);

            properties[1].Name.Should().Be(PropertyName.Items);
            var itemsValue = properties[1].Value.As<StructureValue>();

            for (var i = 0; i < value.Items.Length; i++)
            {
                var setItem = value.Items[i];
                itemsValue.Properties[i].Name.Should().Be(setItem.Item);
                var setItemValue = itemsValue.Properties[i].Value.As<StructureValue>();
                AssertCounterValueSetItem(setItemValue.Properties, setItem);
            }
        }

        [Test]
        public void GaugeValuePropertyBuilder()
        {
            const double value = Test.Gauge.Value;

            var properties = value.BuildProperties().ToList();

            properties.Should().HaveCount(1);

            properties[0].Name.Should().Be(PropertyName.Value);
            properties[0].Value.As<ScalarValue>().Value.Should().Be(value);
        }

        [Test]
        public void GaugeValuePropertyBuilderWithNaN()
        {
            Test.Gauge.NaN.BuildProperties().Should().BeEmpty();
        }

        [Test]
        public void GaugeValuePropertyBuilderWithPositiveInfinity()
        {
            Test.Gauge.PositiveInfinity.BuildProperties().Should().BeEmpty();
        }

        [Test]
        public void GaugeValuePropertyBuilderWithNegativeInfinity()
        {
            Test.Gauge.NegativeInfinity.BuildProperties().Should().BeEmpty();
        }

        [Theory]
        public void TimeUnitPropertyBuilder(TimeUnit timeUnit)
        {
            var properties = timeUnit.BuildProperties("Foo").ToList();

            properties.Should().HaveCount(1);

            properties[0].Name.Should().Be("Foo");
            properties[0].Value.As<ScalarValue>().Value.Should().Be(timeUnit);
        }

        [Test]
        public void UnitPropertyBuilder()
        {
            var unit = Unit.Commands;

            var properties = unit.BuildProperties().ToList();

            properties.Should().HaveCount(1);

            properties[0].Name.Should().Be(PropertyName.Unit);
            properties[0].Value.As<ScalarValue>().Value.Should().Be(unit.Name);
        }

        [Test]
        public void HealthStatus()
        {
            var value = Test.HealthStatus.WithTwoResults;
            var result1 = value.Results[0];
            var result2 = value.Results[1];

            var properties = value.BuildProperties().ToList();

            properties.Should().HaveCount(4);

            properties[0].Name.Should().Be(PropertyName.HasRegisteredChecks);
            properties[0].Value.As<ScalarValue>().Value.Should().Be(value.HasRegisteredChecks);

            properties[1].Name.Should().Be(PropertyName.IsHealthy);
            properties[1].Value.As<ScalarValue>().Value.Should().Be(value.IsHealthy);

            properties[2].Name.Should().Be(result1.Name);
            var result1Value = properties[2].Value.As<StructureValue>();
            AssertHealthCheckResult(result1Value.Properties, result1);

            properties[3].Name.Should().Be(result2.Name);
            var result2Value = properties[3].Value.As<StructureValue>();
            AssertHealthCheckResult(result2Value.Properties, result2);
        }

        [Test]
        public void HealthStatusWithNoRegisteredChecks()
        {
            var value = Test.HealthStatus.WithNoResults;

            var properties = value.BuildProperties().ToList();

            properties.Should().HaveCount(1);

            properties[0].Name.Should().Be(PropertyName.HasRegisteredChecks);
            properties[0].Value.As<ScalarValue>().Value.Should().Be(value.HasRegisteredChecks);
        }

        [Test]
        public void HistogramValue()
        {
            var value = Test.Histogram.Value;

            var properties = value.BuildProperties().ToList();

            AssertHistogramValue(properties, value);
        }

        [Test]
        public void MeterValue()
        {
            var value = Test.Meter.WithTwoItems;

            var properties = value.BuildProperties().ToList();

            AssertMeterValue(properties, value);
        }

        [Test]
        public void MeterValueWithNoItems()
        {
            var value = Test.Meter.WithNoItems;

            var properties = value.BuildProperties().ToList();

            AssertMeterValue(properties, value);
        }

        [Test]
        public void MetricTags()
        {
            var value = Test.Tags.ThreeTags;

            var properties = value.BuildProperties().ToList();

            properties.Should().HaveCount(1);

            properties[0].Name.Should().Be(PropertyName.Tags);
            var values = properties[0].Value.As<SequenceValue>();
            values.Elements.Should().HaveCount(3);
            values.Elements[0].As<ScalarValue>().Value.Should().Be(value.Tags[0]);
            values.Elements[1].As<ScalarValue>().Value.Should().Be(value.Tags[1]);
            values.Elements[2].As<ScalarValue>().Value.Should().Be(value.Tags[2]);
        }

        [Test]
        public void MetricTagsEmpty()
        {
            var value = Test.Tags.Empty;

            value.BuildProperties().Should().BeEmpty();
        }

        [Test]
        public void MeterValueSetItem()
        {
            var value = Test.Meter.WithTwoItems.Items[0];

            var properties = value.BuildProperties().ToList();

            AssertMeterValueSetItem(properties, value);
        }

        [Test]
        public void MeterValueSetItemsArray()
        {
            var value = Test.Meter.WithTwoItems.Items;
            var item1 = value[0];
            var item2 = value[1];

            var properties = value.BuildProperties().ToList();

            properties.Should().HaveCount(2);

            properties[0].Name.Should().Be(item1.Item);
            var item1Value = properties[0].Value.As<StructureValue>();
            AssertMeterValueSetItem(item1Value.Properties, item1);

            properties[1].Name.Should().Be(item2.Item);
            var item2Value = properties[1].Value.As<StructureValue>();
            AssertMeterValueSetItem(item2Value.Properties, item1);
        }

        [Test]
        public void TimerValue()
        {
            var value = Test.Timer.Value;

            var properties = value.BuildProperties().ToList();

            properties.Should().HaveCount(4);

            properties[0].Name.Should().Be(PropertyName.ActiveSessions);
            properties[0].Value.As<ScalarValue>().Value.Should().Be(value.ActiveSessions);

            properties[1].Name.Should().Be(PropertyName.Histogram);
            var histogramValue = properties[1].Value.As<StructureValue>();
            AssertHistogramValue(histogramValue.Properties, value.Histogram);

            properties[2].Name.Should().Be(PropertyName.Rate);
            var rateValue = properties[2].Value.As<StructureValue>();
            AssertMeterValue(rateValue.Properties, value.Rate);

            properties[3].Name.Should().Be(PropertyName.TotalTime);
            properties[3].Value.As<ScalarValue>().Value.Should().Be(value.TotalTime);
        }

        private static void AssertCounterValueSetItem(IReadOnlyList<LogEventProperty> properties, CounterValue.SetItem value)
        {
            properties.Should().HaveCount(2);

            properties[0].Name.Should().Be(PropertyName.Count);
            properties[0].Value.As<ScalarValue>().Value.Should().Be(value.Count);

            properties[1].Name.Should().Be(PropertyName.Percent);
            properties[1].Value.As<ScalarValue>().Value.Should().Be(value.Percent);
        }

        private static void AssertHealthCheckResult(IReadOnlyList<LogEventProperty> properties, HealthCheck.Result value)
        {
            properties.Should().HaveCount(2);

            properties[0].Name.Should().Be(PropertyName.IsHealthy);
            properties[0].Value.As<ScalarValue>().Value.Should().Be(value.Check.IsHealthy);

            properties[1].Name.Should().Be(PropertyName.Message);
            properties[1].Value.As<ScalarValue>().Value.Should().Be(value.Check.Message);
        }

        private static void AssertHistogramValue(IReadOnlyList<LogEventProperty> properties, HistogramValue value)
        {
            properties.Should().HaveCount(16);

            properties[0].Name.Should().Be(PropertyName.Count);
            properties[0].Value.As<ScalarValue>().Value.Should().Be(value.Count);

            properties[1].Name.Should().Be(PropertyName.LastUserValue);
            properties[1].Value.As<ScalarValue>().Value.Should().Be(value.LastUserValue);

            properties[2].Name.Should().Be(PropertyName.LastValue);
            properties[2].Value.As<ScalarValue>().Value.Should().Be(value.LastValue);

            properties[3].Name.Should().Be(PropertyName.Max);
            properties[3].Value.As<ScalarValue>().Value.Should().Be(value.Max);

            properties[4].Name.Should().Be(PropertyName.MaxUserValue);
            properties[4].Value.As<ScalarValue>().Value.Should().Be(value.MaxUserValue);

            properties[5].Name.Should().Be(PropertyName.Mean);
            properties[5].Value.As<ScalarValue>().Value.Should().Be(value.Mean);

            properties[6].Name.Should().Be(PropertyName.Median);
            properties[6].Value.As<ScalarValue>().Value.Should().Be(value.Median);

            properties[7].Name.Should().Be(PropertyName.Min);
            properties[7].Value.As<ScalarValue>().Value.Should().Be(value.Min);

            properties[8].Name.Should().Be(PropertyName.MinUserValue);
            properties[8].Value.As<ScalarValue>().Value.Should().Be(value.MinUserValue);

            properties[9].Name.Should().Be(PropertyName.Percentile75);
            properties[9].Value.As<ScalarValue>().Value.Should().Be(value.Percentile75);

            properties[10].Name.Should().Be(PropertyName.Percentile95);
            properties[10].Value.As<ScalarValue>().Value.Should().Be(value.Percentile95);

            properties[11].Name.Should().Be(PropertyName.Percentile98);
            properties[11].Value.As<ScalarValue>().Value.Should().Be(value.Percentile98);

            properties[12].Name.Should().Be(PropertyName.Percentile99);
            properties[12].Value.As<ScalarValue>().Value.Should().Be(value.Percentile99);

            properties[13].Name.Should().Be(PropertyName.Percentile999);
            properties[13].Value.As<ScalarValue>().Value.Should().Be(value.Percentile999);

            properties[14].Name.Should().Be(PropertyName.SampleSize);
            properties[14].Value.As<ScalarValue>().Value.Should().Be(value.SampleSize);

            properties[15].Name.Should().Be(PropertyName.StdDev);
            properties[15].Value.As<ScalarValue>().Value.Should().Be(value.StdDev);
        }

        private static void AssertMeterValue(IReadOnlyList<LogEventProperty> properties, MeterValue value)
        {
            properties.Should().HaveCount(6 + (value.Items.Length > 0 ? 1 : 0));

            properties[0].Name.Should().Be(PropertyName.Count);
            properties[0].Value.As<ScalarValue>().Value.Should().Be(value.Count);

            properties[1].Name.Should().Be(PropertyName.FifteenMinuteRate);
            properties[1].Value.As<ScalarValue>().Value.Should().Be(value.FifteenMinuteRate);

            properties[2].Name.Should().Be(PropertyName.FiveMinuteRate);
            properties[2].Value.As<ScalarValue>().Value.Should().Be(value.FiveMinuteRate);

            properties[3].Name.Should().Be(PropertyName.MeanRate);
            properties[3].Value.As<ScalarValue>().Value.Should().Be(value.MeanRate);

            properties[4].Name.Should().Be(PropertyName.OneMinuteRate);
            properties[4].Value.As<ScalarValue>().Value.Should().Be(value.OneMinuteRate);

            properties[5].Name.Should().Be(PropertyName.RateUnit);
            properties[5].Value.As<ScalarValue>().Value.Should().Be(value.RateUnit);

            if (value.Items.Length == 0) return;

            properties[6].Name.Should().Be(PropertyName.Items);
            var itemsValue = properties[6].Value.As<StructureValue>();

            for (var i = 0; i < value.Items.Length; i++)
            {
                var setItem = value.Items[i];
                itemsValue.Properties[i].Name.Should().Be(setItem.Item);
                var setItemValue = itemsValue.Properties[i].Value.As<StructureValue>();
                AssertMeterValueSetItem(setItemValue.Properties, setItem);
            }
        }

        private static void AssertMeterValueSetItem(IReadOnlyList<LogEventProperty> properties, MeterValue.SetItem item)
        {
            properties.Should().HaveCount(7);

            properties[0].Name.Should().Be(PropertyName.Count);
            properties[0].Value.As<ScalarValue>().Value.Should().Be(item.Value.Count);

            properties[1].Name.Should().Be(PropertyName.FifteenMinuteRate);
            properties[1].Value.As<ScalarValue>().Value.Should().Be(item.Value.FifteenMinuteRate);

            properties[2].Name.Should().Be(PropertyName.FiveMinuteRate);
            properties[2].Value.As<ScalarValue>().Value.Should().Be(item.Value.FiveMinuteRate);

            properties[3].Name.Should().Be(PropertyName.MeanRate);
            properties[3].Value.As<ScalarValue>().Value.Should().Be(item.Value.MeanRate);

            properties[4].Name.Should().Be(PropertyName.OneMinuteRate);
            properties[4].Value.As<ScalarValue>().Value.Should().Be(item.Value.OneMinuteRate);

            properties[5].Name.Should().Be(PropertyName.RateUnit);
            properties[5].Value.As<ScalarValue>().Value.Should().Be(item.Value.RateUnit);

            properties[6].Name.Should().Be(PropertyName.Percent);
            properties[6].Value.As<ScalarValue>().Value.Should().Be(item.Percent);
        }
    }
}