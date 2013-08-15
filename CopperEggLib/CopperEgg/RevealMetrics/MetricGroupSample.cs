using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CopperEggLib
{
    /// <summary>
    /// Represents a single sample to store with a <see cref="MetricGroup"/>.
    /// </summary>
    public class MetricGroupSample
    {
        /// <summary>
        /// Gets or sets the identifier of this metric group sample. This is normally an identifying machine, server, or source of the sample.
        /// </summary>
        [JsonProperty( "identifier" )]
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the time this sample was collected.
        /// </summary>
        [JsonProperty( "timestamp" )]
        [JsonConverter( typeof( UnixTimeConverter ) )]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the values of this sample.
        /// </summary>
        [JsonProperty( "values" )]
        public Dictionary<string, object> Values { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="MetricGroupSample"/> class.
        /// </summary>
        public MetricGroupSample()
        {
            Values = new Dictionary<string, object>();

            Timestamp = DateTime.UtcNow;
            Identifier = Environment.MachineName;
        }
    }
}