using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopperEggLib;

namespace CoppereggMetrics
{
    abstract class BaseMetricProvider<TSource> : IMetricProvider
    {
        MetricGroup metricGroup;


        public abstract string MetricName { get; }
        public abstract string MetricLabel { get; }


        protected abstract void SetupMetricGroup( MetricGroup metricGroup );

        protected abstract Task<TSource> GetMetricSourceData();
        protected abstract MetricGroupSample ProcessMetricSourceData( MetricGroup metricGroup, TSource source );


        public MetricGroup GetMetricGroup()
        {
            metricGroup = new MetricGroup
            {
                Name = MetricName,
                Label = MetricLabel,
            };

            SetupMetricGroup( metricGroup );

            return metricGroup;
        }

        public async Task<MetricGroupSample> PerformSample()
        {
            var sourceData = await GetMetricSourceData();

            return ProcessMetricSourceData( metricGroup, sourceData );
        }
    }
}
