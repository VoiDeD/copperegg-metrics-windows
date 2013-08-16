using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopperEggLib;

namespace CoppereggMetrics
{
    class NETMetricProvider : BaseMetricProvider<Dictionary<string, object>>
    {
        Dictionary<string, PerformanceCounter> perfCounterMap = new Dictionary<string, PerformanceCounter>();


        public override string MetricName { get { return "dotnet_metrics"; } }
        public override string MetricLabel { get { return ".NET Metrics"; } }


        protected override void SetupMetricGroup( MetricGroup metricGroup )
        {
            SetupMetric( metricGroup, @".NET CLR Exceptions\# of Exceps Thrown\_Global_", "Exceptions Thrown", MetricType.Counter, "Exceptions" );
        }

        protected override Task<Dictionary<string, object>> GetMetricSourceData()
        {
            var data = new Dictionary<string, object>();

            foreach ( var kvp in perfCounterMap )
            {
                data[ kvp.Key ] = kvp.Value.NextValue();
            }

            return Task.FromResult( data );
        }

        protected override MetricGroupSample ProcessMetricSourceData( MetricGroup metricGroup, Dictionary<string, object> source )
        {
            var sample = new MetricGroupSample();

            foreach ( var metric in metricGroup.Metrics )
            {
                switch ( metric.Type )
                {
                    case MetricType.Counter:
                        sample.Values[ metric.Name ] = Convert.ToInt64( source[ metric.Name ] );
                        break;

                    // todo: more types
                }
            }

            return sample;
        }


        void SetupMetric( MetricGroup metricGroup, string perfCounter, string label, MetricType type, string units )
        {
            metricGroup.AddMetric( perfCounter, label, type, units );

            string[] counterSplits = perfCounter.Split( '\\' );
            var counterObj = new PerformanceCounter( counterSplits[ 0 ], counterSplits[ 1 ], counterSplits[ 2 ] );

            perfCounterMap[ perfCounter ] = counterObj;
        }

    }
}
