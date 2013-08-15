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
    class MySQLMetrics : IMetricProvider
    {
        public MetricGroup SetupMetricGroup()
        {
            var mg = new MetricGroup
            {
                Name = "mysql_metrics",
                Label = "MySQL Metrics",
            };

            mg.AddMetric( "threads", "Threads Connected", MetricType.Gauge, "Threads" );
            mg.AddMetric( "queries", "Queries", MetricType.Counter, "Queries" );
            mg.AddMetric( "slow_queries", "Slow Queries", MetricType.Counter, "Queries" );
            mg.AddMetric( "insert_commands", "Insert Commands", MetricType.Counter, "Commands" );
            mg.AddMetric( "select_commands", "Select Commands", MetricType.Counter, "Commands" );
            mg.AddMetric( "update_commands", "Update Commands", MetricType.Counter, "Commands" );
            mg.AddMetric( "uptime", "Uptime", MetricType.Counter, "Seconds" );
            mg.AddMetric( "max_used_connections", "Peak Used Connections", MetricType.Gauge, "Connections" );
            mg.AddMetric( "select_full_join", "Full Scan Joins", MetricType.Counter, "Queries" );
            mg.AddMetric( "bytes_received", "Data Received", MetricType.Counter, "Bytes" );
            mg.AddMetric( "bytes_sent", "Data Sent", MetricType.Counter, "Bytes" );
            mg.AddMetric( "connections", "Connections", MetricType.Counter, "Connections" );
            mg.AddMetric( "threads_running", "Threads Running", MetricType.Gauge, "Threads" );

            return mg;
        }

        public async Task<MetricGroupSample> PerformSample()
        {
            using ( var conn = new MySqlConnection( Settings.Current.MySQLConnectionString ) )
            {
                await conn.OpenAsync();

                using ( var cmd = conn.CreateCommand() )
                {
                    cmd.CommandText = "SHOW GLOBAL STATUS";

                    DbDataReader reader = await cmd.ExecuteReaderAsync();

                    var rows = new Dictionary<string, object>();

                    while ( await reader.ReadAsync() )
                    {
                        rows[ reader.GetString( 0 ) ] = reader.GetValue( 1 );
                    }

                    var sample = new MetricGroupSample();

                    sample.Values[ "threads" ] = Convert.ToInt32( rows[ "Threads_connected" ] );
                    sample.Values[ "queries" ] = Convert.ToInt32( rows[ "Queries" ] );
                    sample.Values[ "slow_queries" ] = Convert.ToInt32( rows[ "Slow_queries" ] );
                    sample.Values[ "insert_commands" ] = Convert.ToInt32( rows[ "Com_insert" ] );
                    sample.Values[ "select_commands" ] = Convert.ToInt32( rows[ "Com_select" ] );
                    sample.Values[ "update_commands" ] = Convert.ToInt32( rows[ "Com_update" ] );
                    sample.Values[ "uptime" ] = Convert.ToInt32( rows[ "Uptime" ] );
                    sample.Values[ "max_used_connections" ] = Convert.ToInt32( rows[ "Max_used_connections" ] );
                    sample.Values[ "select_full_join" ] = Convert.ToInt32( rows[ "Select_full_join" ] );
                    sample.Values[ "bytes_received" ] = Convert.ToInt32( rows[ "Bytes_received" ] );
                    sample.Values[ "bytes_sent" ] = Convert.ToInt32( rows[ "Bytes_sent" ] );
                    sample.Values[ "connections" ] = Convert.ToInt32( rows[ "Connections" ] );
                    sample.Values[ "threads_running" ] = Convert.ToInt32( rows[ "Threads_running" ] );

                    return sample;
                }
            }
        }
    }
}
