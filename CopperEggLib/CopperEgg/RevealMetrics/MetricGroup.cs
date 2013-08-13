using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CopperEggLib
{
    public class MetricGroup
    {
        internal CopperEgg Client { get; set; }


        public string ID { get; set; }

        public string Name { get; set; }
        public string Label { get; set; }

        [JsonConverter( typeof( TimeSpanSecondsConverter ) )]
        public TimeSpan Frequency { get; set; }

        public List<Metric> Metrics { get; set; }


        public Task Delete()
        {
            return Client.DeleteMetricGroup( this.ID );
        }
    }

    public class Metric
    {
        public string Type { get; set; }

        public string Name { get; set; }
        public string Label { get; set; }

        public int Position { get; set; }

        public string Units { get; set; }
    }
}
