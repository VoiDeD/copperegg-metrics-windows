using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CopperEggLib
{
    public class MetricGroup
    {
        internal CopperEgg Client { get; set; }


        [JsonProperty( "id" )]
        public string ID { get; set; }

        [JsonProperty( "name" )]
        public string Name { get; set; }
        [JsonProperty( "label" )]
        public string Label { get; set; }

        [JsonConverter( typeof( TimeSpanSecondsConverter ) )]
        [JsonProperty( "frequency" )]
        public TimeSpan Frequency { get; set; }

        [JsonProperty( "metrics" )]
        public List<Metric> Metrics { get; set; }


        public MetricGroup()
        {
            Metrics = new List<Metric>();
        }


        public Task Delete()
        {
            return Client.DeleteMetricGroup( this.ID );
        }


        public Metric CreateMetric()
        {
            var metric = new Metric();

            Metrics.Add( metric );

            return metric;
        }
    }

    public enum MetricType
    {
        Invalid,

        [EnumMember( Value = "ce_counter" )]
        Counter,
        [EnumMember( Value = "ce_gauge" )]
        Gauge,
        [EnumMember( Value = "ce_gauge_f" )]
        GaugeFloat,
    }

    public class Metric
    {
        [JsonConverter( typeof( StringToEnumConverter ) )]
        [JsonProperty( "type" )]
        public MetricType Type { get; set; }

        [JsonProperty( "name" )]
        public string Name { get; set; }
        [JsonProperty( "label" )]
        public string Label { get; set; }

        [JsonProperty( "position" )]
        public int Position { get; set; }

        [JsonProperty( "units" )]
        public string Units { get; set; }
    }
}