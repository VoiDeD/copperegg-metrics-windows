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
    /// <summary>
    /// Represents the various types of probes.
    /// </summary>
    public enum ProbeType
    {
        /// <summary>
        /// A probe type that will perform an HTTP(S) GET test.
        /// </summary>
        GET,
        /// <summary>
        /// A probe type that will perform an HTTP(S) PUT test.
        /// </summary>
        PUT,
        /// <summary>
        /// A probe type that will perform a TCP connection.
        /// </summary>
        TCP,
        /// <summary>
        /// A probe type that will perform an ICMP ping.
        /// </summary>
        ICMP
    }

    /// <summary>
    /// Represents the state of a given probe.
    /// </summary>
    public enum ProbeState
    {
        /// <summary>
        /// The probe is enabled.
        /// </summary>
        Enabled,
        /// <summary>
        /// The probe is disabled.
        /// </summary>
        Disabled
    }

    /// <summary>
    /// Represents a single probe within the CopperEgg system.
    /// </summary>
    public class Probe
    {
        internal CopperEgg Client { get; set; }


        /// <summary>
        /// Gets or sets the ID of this probe.
        /// </summary>
        [JsonProperty( "id" )]
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets a user defined description of this probe.
        /// </summary>
        [JsonProperty( "probe_desc" )]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a URL or IP address of the destination being tested by this probe.
        /// </summary>
        [JsonProperty( "probe_dest" )]
        public string Destination { get; set; }

        /// <summary>
        /// Gets or sets the frequency at which this probe performs a test.
        /// </summary>
        [JsonConverter( typeof( TimeSpanSecondsConverter ) )]
        [JsonProperty( "frequency" )]
        public TimeSpan Frequency { get; set; }

        /// <summary>
        /// Gets or sets the timeout for this probe's test.
        /// </summary>
        [JsonConverter( typeof( TimeSpanMillisecondsConverter ) )]
        [JsonProperty( "timeout" )]
        public TimeSpan Timeout { get; set; }

        /// <summary>
        /// Gets or sets the number of retry attempts for the test.
        /// </summary>
        [JsonProperty( "retries" )]
        public int Retries { get; set; }

        /// <summary>
        /// Gets or sets the probe type.
        /// </summary>
        [JsonConverter( typeof( StringEnumConverter ) )]
        [JsonProperty( "type" )]
        public ProbeType Type { get; set; }

        /// <summary>
        /// Gets or sets the time this probe was created.
        /// </summary>
        [JsonProperty( "created_at" )]
        [JsonConverter( typeof( UnixTimeConverter ) )]
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Gets or sets the time this probe was last updated at.
        /// </summary>
        [JsonProperty( "updated_at" )]
        [JsonConverter( typeof( UnixTimeConverter ) )]
        public DateTime UpdatedTime { get; set; }

        /// <summary>
        /// Gets or sets the state of this probe.
        /// </summary>
        [JsonConverter( typeof( StringEnumConverter ) )]
        [JsonProperty( "state" )]
        public ProbeState State { get; set; }

        /// <summary>
        /// Gets or sets a list of tags associated with this probe.
        /// </summary>
        [JsonProperty( "tags" )]
        public List<string> Tags { get; set; }

        /// <summary>
        /// Gets or sets a list of stations from which this probe will perform tests.
        /// </summary>
        [JsonProperty( "stations" )]
        public List<string> Stations { get; set; }


        /// <summary>
        /// Sends a request to the CopperEgg backend to delete this <see cref="Probe"/>.
        /// </summary>
        /// <returns>A <see cref="Task"/> that can be used to monitor the completion of this request.</returns>
        public Task Delete()
        {
            return Client.DeleteProbe( this );
        }

        /// <summary>
        /// Sends a request to the CopperEgg backend to retrie a recent sample from this probe.
        /// </summary>
        /// <returns>A <see cref="Task"/> that can be used to monitor the completion of this request.</returns>
        public async Task<ProbeSample> GetSample()
        {
            var probeList = await Client.GetProbeSamples( EnumerableEx.FromSingle( this ) );
            return probeList.First();
        }
    }
}