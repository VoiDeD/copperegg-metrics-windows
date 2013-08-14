using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace CopperEggLib
{
    /// <summary>
    /// Represents a single data sample reported by a <see cref="Probe"/>.
    /// </summary>
    [JsonConverter( typeof( ProbeSampleConverter ) )]
    public class ProbeSample
    {
        /// <summary>
        /// Gets or sets the ID of the <see cref="Probe"/> this sample belongs to.
        /// </summary>
        [JsonProperty( "id" )]
        public string ProbeID { get; set; }

        /// <summary>
        /// Gets or sets the name of the <see cref="Probe"/> this sample belongs to.
        /// </summary>
        [JsonIgnore]
        [JsonProperty( "name" )]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the action the parent <see cref="Probe"/> performed to acquire this sample.
        /// </summary>
        [JsonIgnore]
        [JsonProperty( "action" )]
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the size of the sample.
        /// </summary>
        [JsonProperty( "_bs" )]
        public int SampleSize { get; set; }

        /// <summary>
        /// Gets or sets the time stamp of this sample.
        /// </summary>
        [JsonProperty( "_ts" )]
        [JsonConverter( typeof( UnixTimeConverter ) )]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the latencies reported by the parent <see cref="Probe"/>.
        /// </summary>
        [JsonProperty( "l" )]
        public Dictionary<int, int[]> Latencies { get; set; }

        /// <summary>
        /// Gets or sets the HTTP status codes reported by the parent <see cref="Probe"/>.
        /// </summary>
        [JsonProperty( "s" )]
        public Dictionary<int, Dictionary<string, int>> StatusCodes { get; set; }

        /// <summary>
        /// Gets or sets the health value reported by the parent <see cref="Probe"/>.
        /// </summary>
        [JsonProperty( "h" )]
        public Dictionary<int, int> Health { get; set; }

        /// <summary>
        /// Gets or sets the uptime percentage reported by the parent <see cref="Probe"/>.
        /// </summary>
        [JsonProperty( "u" )]
        public Dictionary<int, int> Uptime { get; set; }
    }

    class ProbeSampleConverter : CustomCreationConverter<ProbeSample>
    {
        public override ProbeSample Create( Type objectType )
        {
            return new ProbeSample();
        }

        public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
        {
            var obj = JObject.Load( reader );

            ProbeSample sample = ( ProbeSample )base.ReadJson( obj.CreateReader(), objectType, existingValue, serializer );

            sample.Name = ( string )obj[ "a" ][ "n" ];
            sample.Action = ( string )obj[ "a" ][ "c" ];

            return sample;
        }
    }
}
