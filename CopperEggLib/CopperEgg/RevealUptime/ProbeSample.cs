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
    [JsonConverter( typeof( ProbeSampleConverter ) )]
    public class ProbeSample
    {
        [JsonProperty( "id" )]
        public string ProbeID { get; set; }

        [JsonIgnore]
        [JsonProperty( "name" )]
        public string Name { get; set; }

        [JsonIgnore]
        [JsonProperty( "action" )]
        public string Action { get; set; }

        [JsonProperty( "_bs" )]
        public int SampleSize { get; set; }

        [JsonProperty( "_ts" )]
        [JsonConverter( typeof( UnixTimeConverter ) )]
        public DateTime Timestamp { get; set; }

        [JsonProperty( "l" )]
        public Dictionary<int, int[]> Latencies { get; set; }

        [JsonProperty( "s" )]
        public Dictionary<int, Dictionary<string, int>> StatusCodes { get; set; }

        [JsonProperty( "h" )]
        public Dictionary<int, int> Health { get; set; }

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
