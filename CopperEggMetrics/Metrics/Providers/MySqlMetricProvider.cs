using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopperEggLib;
using MySql.Data.MySqlClient;

namespace CoppereggMetrics
{
    class MySqlMetricProvider : BaseMetricProvider<Dictionary<string, object>>
    {
        public override string MetricName { get { return "mysql_metrics"; } }
        public override string MetricLabel { get { return "MySQL Metrics"; }  }


        protected override void SetupMetricGroup( MetricGroup metricGroup )
        {
            metricGroup.AddMetric( "threads_connected", "Threads Connected", MetricType.Gauge, "Threads" );
            metricGroup.AddMetric( "queries", "Queries", MetricType.Counter, "Queries" );
            metricGroup.AddMetric( "slow_queries", "Slow Queries", MetricType.Counter, "Queries" );
            metricGroup.AddMetric( "com_insert", "Insert Commands", MetricType.Counter, "Commands" );
            metricGroup.AddMetric( "com_select", "Select Commands", MetricType.Counter, "Commands" );
            metricGroup.AddMetric( "com_update", "Update Commands", MetricType.Counter, "Commands" );
            metricGroup.AddMetric( "uptime", "Uptime", MetricType.Counter, "Seconds" );
            metricGroup.AddMetric( "max_used_connections", "Peak Used Connections", MetricType.Gauge, "Connections" );
            metricGroup.AddMetric( "select_full_join", "Full Scan Joins", MetricType.Counter, "Queries" );
            metricGroup.AddMetric( "bytes_received", "Data Received", MetricType.Counter, "Bytes" );
            metricGroup.AddMetric( "bytes_sent", "Data Sent", MetricType.Counter, "Bytes" );
            metricGroup.AddMetric( "connections", "Connections", MetricType.Counter, "Connections" );
            metricGroup.AddMetric( "threads_running", "Threads Running", MetricType.Gauge, "Threads" );
        }

        protected async override Task<Dictionary<string, object>> GetMetricSourceData()
        {
            using ( var conn = new MySqlConnection( Settings.Current.MySQLConnectionString ) )
            {
                await conn.OpenAsync();

                using ( var cmd = conn.CreateCommand() )
                {
                    cmd.CommandText = "SHOW GLOBAL STATUS";

                    DbDataReader reader = await cmd.ExecuteReaderAsync();

                    var rows = new Dictionary<string, object>( StringComparer.OrdinalIgnoreCase );

                    while ( await reader.ReadAsync() )
                    {
                        rows[ reader.GetString( 0 ) ] = reader.GetValue( 1 );
                    }

                    return rows;
                }
            }
        }

        protected override MetricGroupSample ProcessMetricSourceData( MetricGroup metricGroup, Dictionary<string, object> sourceData )
        {
            var sample = new MetricGroupSample();

            foreach ( var metric in metricGroup.Metrics )
            {
                sample.Values[ metric.Name ] = sourceData[ metric.Name ];
            }

            return sample;
        }

    }
}
