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
    /// <summary>
    /// Represents a metric group within the CopperEgg system.
    /// </summary>
    public class MetricGroup
    {
        internal CopperEgg Client { get; set; }


        /// <summary>
        /// Gets or sets the ID of the metric group.
        /// </summary>
        [JsonProperty( "id" )]
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the internal name of this metric group.
        /// </summary>
        [JsonProperty( "name" )]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the display label for this metric group.
        /// </summary>
        [JsonProperty( "label" )]
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the frequency at which this metric group will be updated.
        /// </summary>
        [JsonConverter( typeof( TimeSpanSecondsConverter ) )]
        [JsonProperty( "frequency" )]
        public TimeSpan Frequency { get; set; }

        /// <summary>
        /// Gets or sets a list of <see cref="Metric"/>s associted with this metric group.
        /// </summary>
        [JsonProperty( "metrics" )]
        public List<Metric> Metrics { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="MetricGroup"/> class.
        /// </summary>
        public MetricGroup()
        {
            Metrics = new List<Metric>();
        }


        /// <summary>
        /// Sends a request to the CopperEgg backend to delete this metric group.
        /// </summary>
        /// <returns>A <see cref="Task"/> that can be used to monitor the completion of this request.</returns>
        public Task Delete()
        {
            if ( string.IsNullOrEmpty( ID ) )
                throw new InvalidOperationException( "Cannot delete a metric group that has not been assigned an ID" );

            return Client.DeleteMetricGroup( this );
        }

        /// <summary>
        /// Stores a single <see cref="MetricGroupSample"/> for this metric group.
        /// </summary>
        /// <param name="sample">The sample to store.</param>
        /// <returns>A <see cref="Task"/> that can be used to monitor the completion of this request.</returns>
        public Task StoreSample( MetricGroupSample sample )
        {
            return Client.StoreMetricGroupSample( this, sample );
        }

        /// <summary>
        /// Creates a new <see cref="Metric"/> and adds it to the <see cref="Metrics"/> list before returning it.
        /// </summary>
        /// <returns>A newly created <see cref="Metric"/> for this <see cref="MetricGroup"/>.</returns>
        public Metric AddMetric( string name = default( string ), string label = default( string ), MetricType type = default( MetricType ), string units = default( string )  )
        {
            var metric = new Metric
            {
                Name = name,
                Label = label,
                Type = type,
                Units = units,
            };

            Metrics.Add( metric );

            return metric;
        }
    }

    /// <summary>
    /// Represents various metric types.
    /// </summary>
    public enum MetricType
    {
        /// <summary>
        /// An invalid metric type.
        /// </summary>
        Invalid,

        /// <summary>
        /// A counter metric type.
        /// </summary>
        [EnumMember( Value = "ce_counter" )]
        Counter,
        /// <summary>
        /// A gauge metric type.
        /// </summary>
        [EnumMember( Value = "ce_gauge" )]
        Gauge,
        /// <summary>
        /// A floating point gauge metric type.
        /// </summary>
        [EnumMember( Value = "ce_gauge_f" )]
        GaugeFloat,
    }

    /// <summary>
    /// Represents a single metric within a <see cref="MetricGroup"/>.
    /// </summary>
    public class Metric
    {
        /// <summary>
        /// Gets or sets the type for this metric.
        /// </summary>
        [JsonConverter( typeof( StringToEnumConverter ) )]
        [JsonProperty( "type" )]
        public MetricType Type { get; set; }

        /// <summary>
        /// Gets or sets the internal name of this metric.
        /// </summary>
        [JsonProperty( "name" )]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the display label for this metric.
        /// </summary>
        [JsonProperty( "label" )]
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the position of this metric within the list of metrics in the parent <see cref="MetricGroup"/>.
        /// </summary>
        [JsonProperty( "position" )]
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets the quantitative unit of this metric.
        /// </summary>
        [JsonProperty( "unit" )]
        public string Units { get; set; }
    }
}