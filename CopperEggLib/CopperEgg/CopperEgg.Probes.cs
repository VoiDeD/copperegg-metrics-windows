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

        public Task<List<ProbeSample>> GetProbeSamples( IEnumerable<Probe> probes )
        {
            if ( !probes.All( p => p.Client == this ) )
                throw new InvalidOperationException( "Cannot request probe sample of a probe not owned by this CopperEgg instance" );

            return GetProbeSamples( probes.Select( p => p.ID ) );
        }
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

        public async Task DeleteProbe( string probeId )
        {
            using ( var apiClient = GetAPIClient() )
            {
                await apiClient.Request( string.Format( "revealuptime/probes/{0}.json", probeId ), HttpMethod.Delete );
            }
        }
    }
}
