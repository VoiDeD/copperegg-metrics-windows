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
            SetupMetric( metricGroup, @".NET CLR Exceptions\# of Exceps Thrown\_Global_", "Exceptions: Exceptions Thrown", MetricType.Counter, "Exceptions" );
            SetupMetric( metricGroup, @".NET CLR Interop\# of marshalling\_Global_", "Interop: Objects Marshalled", MetricType.Counter, "Objects" );
            SetupMetric( metricGroup, @".NET CLR Memory\# GC Handles\_Global_", "Memory: # GC Handles", MetricType.Gauge, "GC Handles" );
            SetupMetric( metricGroup, @".NET CLR Memory\# Total committed Bytes\_Global_", "Memory: Total Committed Bytes", MetricType.Gauge, "Bytes" );
            SetupMetric( metricGroup, @".NET CLR Memory\Allocated Bytes/sec\_Global_", "Memory: Allocated Bytes/sec", MetricType.Gauge, "Bytes/sec" );
            SetupMetric( metricGroup, @".NET CLR Memory\Large Object Heap size\_Global_", "Memory: Large Object Heap Size", MetricType.Gauge, "Bytes" );
            SetupMetric( metricGroup, @".NET CLR Networking\Bytes Received\_global_", "Networking: Bytes Received", MetricType.Counter, "Bytes" );
            SetupMetric( metricGroup, @".NET CLR Networking\Bytes Sent\_global_", "Networking: Bytes Sent", MetricType.Counter, "Bytes" );
            SetupMetric( metricGroup, @".NET CLR Networking\Connections Established\_global_", "Networking: Connections Established", MetricType.Counter, "Sockets" );
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
                object val = null;

                switch ( metric.Type )
                {
                    case MetricType.Counter:
                    case MetricType.Gauge:
                        val = Convert.ToInt64( source[ metric.Name ] );
                        break;

                    // todo: more types?
                    default:
                        Debug.Assert( false );
                        break;
                }

                sample.Values[ metric.Name ] = val;
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
