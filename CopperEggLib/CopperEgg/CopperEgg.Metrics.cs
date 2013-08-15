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
        /// <summary>
        /// Sends a request to the CopperEgg backend to creates a metric group from the given <see cref="MetricGroup"/> information.
        /// </summary>
        /// <param name="groupInfo">The metric group information.</param>
        /// <returns>A <see cref="Task{T}"/> that can be used to monitor the completion of this request.</returns>
        public async Task<MetricGroup> CreateMetricGroup( MetricGroup groupInfo )
        {
            using ( var apiClient = GetAPIClient() )
            {
                var metricGroup = await apiClient.Request<MetricGroup>( "revealmetrics/metric_groups.json", HttpMethod.Post, groupInfo );

                metricGroup.Client = this;

                return metricGroup;
            }
        }


        /// <summary>
        /// Requests a list of all <see cref="MetricGroup"/>s from the CopperEgg backend.
        /// </summary>
        /// <returns>A <see cref="Task{T}"/> that can be used to monitor the completion of this request.</returns>
        public async Task<List<MetricGroup>> GetMetricGroups()
        {
            using ( var apiClient = GetAPIClient() )
            {
                var metricGroups = await apiClient.Request<List<MetricGroup>>( "revealmetrics/metric_groups.json" );

                metricGroups.ForEach( g => g.Client = this );

                return metricGroups;
            }
        }
        /// <summary>
        /// Sends a request to the CopperEgg backend to retrieve information for a single <see cref="MetricGroup"/>.
        /// </summary>
        /// <param name="groupId">The metric group ID to request information for.</param>
        /// <returns>A <see cref="Task"/> that can be used to monitor the completion of this request.</returns>
        public async Task<MetricGroup> GetMetricGroup( string groupId )
        {
            using ( var apiClient = GetAPIClient() )
            {
                var metricGroup = await apiClient.Request<MetricGroup>( string.Format( "revealmetrics/metric_groups/{0}.json", groupId ) );

                metricGroup.Client = this;

                return metricGroup;
            }
        }


        /// <summary>
        /// Stores a single <see cref="MetricGroupSample"/> for the given <see cref="MetricGroup"/>.
        /// </summary>
        /// <param name="metricGroup">The metric group to store the sample for.</param>
        /// <param name="sample">The sample to store.</param>
        /// <returns>A <see cref="Task"/> that can be used to monitor the completion of this request.</returns>
        /// <exception cref="System.InvalidOperationException">A metric group was provided in that does not belong to this CopperEgg instance.</exception>
        public Task StoreMetricGroupSample( MetricGroup metricGroup, MetricGroupSample sample )
        {
            if ( metricGroup.Client != this )
                throw new InvalidOperationException( "Cannot store metric group sample of a metric group not owned by this CopperEgg instance" );

            return StoreMetricGroupSample( metricGroup.ID, sample );
        }
        /// <summary>
        /// Stores a single <see cref="MetricGroupSample"/> for the given metric group ID.
        /// </summary>
        /// <param name="groupId">The metric group ID to store the sample for.</param>
        /// <param name="sample">The <see cref="MetricGroupSample"/> to store.</param>
        /// <returns>A <see cref="Task"/> that can be used to monitor the completion of this request.</returns>
        public async Task StoreMetricGroupSample( string groupId, MetricGroupSample sample )
        {
            using ( var apiClient = GetAPIClient() )
            {
                await apiClient.Request( string.Format( "revealmetrics/samples/{0}.json", groupId ), HttpMethod.Post, sample );
            }
        }


        /// <summary>
        /// Sends a request to delete the given <see cref="MetricGroup"/>.s
        /// </summary>
        /// <param name="metricGroup">The metric group to delete.</param>
        /// <returns>A <see cref="Task"/> that can be used to monitor the completion of this request.</returns>
        /// <exception cref="System.InvalidOperationException">A metric group was provided in that does not belong to this CopperEgg instance.</exception>
        public Task DeleteMetricGroup( MetricGroup metricGroup )
        {
            if ( metricGroup.Client != this )
                throw new InvalidOperationException( "Cannot delete a metric group not owned by this CopperEgg instance" );

            return DeleteMetricGroup( metricGroup.ID );
        }
        /// <summary>
        /// Sends a request to delete the <see cref="MetricGroup"/> specified by the given metric group ID.
        /// </summary>
        /// <param name="groupId">The ID of the metric group to delete.</param>
        /// <returns>A <see cref="Task"/> that can be used to monitor the completion of this request.</returns>
        public async Task DeleteMetricGroup( string groupId )
        {
            using ( var apiClient = GetAPIClient() )
            {
                await apiClient.Request( string.Format( "revealmetrics/metric_groups/{0}.json", groupId ), HttpMethod.Delete );
            }
        }
    }
}
