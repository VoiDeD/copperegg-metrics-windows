using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CopperEggLib
{
    public enum ProbeType
    {
        GET,
        PUT,
        TCP,
        ICMP
    }

    public enum ProbeState
    {
        Enabled,
        Disabled
    }

    public class Probe
    {
        internal CopperEgg Client { get; set; }


        public string ID { get; set; }

        [JsonProperty( "probe_desc" )]
        public string Description { get; set; }

        [JsonProperty( "probe_dest" )]
        public string Destination { get; set; }

        [JsonConverter( typeof( TimeSpanSecondsConverter ) )]
        public TimeSpan Frequency { get; set; }

        [JsonConverter( typeof( TimeSpanMillisecondsConverter ) )]
        public TimeSpan Timeout { get; set; }

        public int Retries { get; set; }

        [JsonConverter( typeof( StringEnumConverter ) )]
        public ProbeType Type { get; set; }

        [JsonProperty( "created_at" )]
        [JsonConverter( typeof( UnixTimeConverter ) )]
        public DateTime CreationTime { get; set; }

        [JsonProperty( "updated_at" )]
        [JsonConverter( typeof( UnixTimeConverter ) )]
        public DateTime UpdatedTime { get; set; }

        [JsonConverter( typeof( StringEnumConverter ) )]
        public ProbeState State { get; set; }

        public List<string> Tags { get; set; }

        public List<string> Stations { get; set; }


        public Task Delete()
        {
            return Client.DeleteProbe( this.ID );
        }
    }
}