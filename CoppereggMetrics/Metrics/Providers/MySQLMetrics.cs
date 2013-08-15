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

            mg.AddMetric( "threads", "Threads", MetricType.Gauge, "Threads" );

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

                    var sample = new MetricGroupSample
                    {
                        Identifier = Settings.Current.Server,
                    };

                    sample.Values[ "threads" ] = Convert.ToInt32( rows[ "Threads_connected" ] );

                    return sample;
                }
            }
        }

        void ReadColumnIntoSample( MetricGroupSample sample, DbDataReader reader, string columnName )
        {
            var value = reader[ columnName ];
        }
    }
}
