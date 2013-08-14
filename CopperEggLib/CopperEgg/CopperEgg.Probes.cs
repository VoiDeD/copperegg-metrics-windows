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
        /// Requests a list of all <see cref="Probe"/>s from the CopperEgg backend.
        /// </summary>
        /// <returns>A <see cref="Task{T}"/> that can be used to monitor the completion of this request.</returns>
        public async Task<List<Probe>> GetProbes()
        {
            using ( var apiClient = GetAPIClient() )
            {
                var probes = await apiClient.Request<List<Probe>>( "revealuptime/probes.json" );

                if ( probes == null )
                    return null;

                probes.ForEach( probe => probe.Client = this );

                return probes;
            }
        }

        /// <summary>
        /// Sends a request to the CopperEgg backend to retrieve information for a single <see cref="Probe"/>.
        /// </summary>
        /// <param name="probeId">The probe id.</param>
        /// <returns>A <see cref="Task"/> that can be used to monitor the completion of this request.</returns>
        public async Task<Probe> GetProbe( string probeId )
        {
            using ( var apiClient = GetAPIClient() )
            {
                var probe = await apiClient.Request<Probe>( string.Format( "revealuptime/probes/{0}.json", probeId ) );

                if ( probe == null )
                    return null;

                probe.Client = this;

                return probe;
            }
        }

        /// <summary>
        /// Requests a list of <see cref="ProbeSample"/>s for the given <see cref="Probe"/>s.
        /// </summary>
        /// <param name="probes">The probes for which to reqest samples.</param>
        /// <returns>A <see cref="Task{T}"/> that can be used to monitor the completion of this request.</returns>
        /// <exception cref="System.InvalidOperationException">A probe was provided in that does not belong to this CopperEgg instance.</exception>
        public Task<List<ProbeSample>> GetProbeSamples( IEnumerable<Probe> probes )
        {
            if ( !probes.All( p => p.Client == this ) )
                throw new InvalidOperationException( "Cannot request probe sample of a probe not owned by this CopperEgg instance" );

            return GetProbeSamples( probes.Select( p => p.ID ) );
        }
        /// <summary>
        /// Requests a list of <see cref="ProbeSample"/>s for the given probe IDs.
        /// </summary>
        /// <param name="probeIds">The probe IDs for which to reqest samples.</param>
        /// <returns>A <see cref="Task{T}"/> that can be used to monitor the completion of this request.</returns>
        public async Task<List<ProbeSample>> GetProbeSamples( IEnumerable<string> probeIds )
        {
            using ( var apiClient = GetAPIClient() )
            {
                var probeList = string.Join( ",", probeIds );
                apiClient.AddArgument( "ids", probeList );

                var req = string.Format( "revealuptime/samples.json" );

                return await apiClient.Request<List<ProbeSample>>( req );
            }
        }

        /// <summary>
        /// Sends a request to delete the <see cref="Probe"/> specified by rhe given probe ID.
        /// </summary>
        /// <param name="probeId">The ID of the probe to delete.</param>
        /// <returns>A <see cref="Task"/> that can be used to monitor the completion of this request.</returns>
        public async Task DeleteProbe( string probeId )
        {
            using ( var apiClient = GetAPIClient() )
            {
                await apiClient.Request( string.Format( "revealuptime/probes/{0}.json", probeId ), HttpMethod.Delete );
            }
        }
    }
}
