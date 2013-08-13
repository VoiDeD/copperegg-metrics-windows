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
            using ( var apiClient = new APIClient( APIKey ) )
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
            using ( var apiClient = new APIClient( APIKey ) )
            {
                var probe = await apiClient.Request<Probe>( string.Format( "revealuptime/probes/{0}.json", probeId ) );

                if ( probe == null )
                    return null;

                probe.Client = this;

                return probe;
            }
        }

        public Task DeleteProbe( string probeId )
        {
            using ( var apiClient = new APIClient( APIKey ) )
            {
                return apiClient.Request( string.Format( "revealuptime/probes/{0}.json", probeId ), HttpMethod.Delete );
            }
        }
    }
}
