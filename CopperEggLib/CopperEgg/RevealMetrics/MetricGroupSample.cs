using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CopperEggLib
{
    public class MetricGroupSample
    {
        [JsonProperty( "identifier" )]
        public string Identifier { get; set; }

        [JsonProperty( "timestamp" )]
        public DateTime Timestamp { get; set; }

        [JsonProperty( "values" )]
        public Dictionary<string, float> Values { get; set; }
    }
}
