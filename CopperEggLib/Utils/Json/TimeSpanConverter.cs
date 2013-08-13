using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CopperEggLib
{
    abstract class TimeSpanConverter : JsonConverter
    {
        public override bool CanConvert( Type objectType )
        {
            return objectType == typeof( TimeSpan );
        }

        public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
        {
            if ( reader.TokenType != JsonToken.Integer )
                throw new JsonSerializationException( string.Format( "Unexpected token when parsing timespan. Expected integer, got {0}", reader.TokenType ) );

            return ToTimeSpan( ( long )reader.Value );
        }

        public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
        {
            if ( value is TimeSpan )
            {
                writer.WriteValue( FromTimeSpan( ( TimeSpan )value ) );
            }
            else
            {
                throw new JsonSerializationException( "Expected timespan object value" );
            }
        }

        protected abstract TimeSpan ToTimeSpan( long value );
        protected abstract long FromTimeSpan( TimeSpan value );
    }

    class TimeSpanSecondsConverter : TimeSpanConverter
    {
        protected override TimeSpan ToTimeSpan( long value )
        {
            return TimeSpan.FromSeconds( value );
        }

        protected override long FromTimeSpan( TimeSpan value )
        {
            return ( long )value.TotalSeconds;
        }
    }

    class TimeSpanMillisecondsConverter : TimeSpanConverter
    {
        protected override TimeSpan ToTimeSpan( long value )
        {
            return TimeSpan.FromMilliseconds( value );
        }

        protected override long FromTimeSpan( TimeSpan value )
        {
            return ( long )value.TotalMilliseconds;
        }
    }
}
