using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CopperEggLib
{
    public partial class CopperEgg
    {
        public async Task<List<MetricGroup>> GetMetricGroups()
        {
            using ( var apiClient = GetAPIClient() )
            {
                var metricGroups = await apiClient.Request<List<MetricGroup>>( "revealmetrics/metric_groups.json" );

                metricGroups.ForEach( g => g.Client = this );

                return metricGroups;
            }
        }
        public async Task<MetricGroup> GetMetricGroup( string groupId )
        {
            using ( var apiClient = GetAPIClient() )
            {
                var metricGroup = await apiClient.Request<MetricGroup>( string.Format( "revealmetrics/metric_groups/{0}.json", groupId ) );

                metricGroup.Client = this;

                return metricGroup;
            }
        }

        public async Task DeleteMetricGroup( string groupId )
        {
            using ( var apiClient = GetAPIClient() )
            {
                await apiClient.Request( string.Format( "revealmetrics/metric_groups/{0}.json", groupId ), HttpMethod.Delete );
            }
        }
    }
}
