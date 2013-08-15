using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CopperEggLib
{
    class UnixTimeConverter : DateTimeConverterBase 
    {
        static readonly DateTime UnixEpoch = new DateTime( 1970, 1, 1, 0, 0, 0, DateTimeKind.Utc );


        public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
        {
            if ( reader.TokenType != JsonToken.Integer )
            {
                throw new JsonSerializationException( string.Format( "Unexpected token when parsing datetime. Expected integer, got {0}", reader.TokenType ) );
            }

            var seconds = ( long )reader.Value;

            return UnixEpoch.AddSeconds( seconds );
        }

        public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
        {
            if ( value is DateTime )
            {
                DateTime dateValue = ( DateTime )value;

                var diff = dateValue - UnixEpoch;

                if ( diff.TotalSeconds < 0 )
                {
                    throw new JsonSerializationException( "Invalid unix timestamp" );
                }

                writer.WriteValue( ( long )diff.TotalSeconds );
            }
            else
            {
                throw new JsonSerializationException( "Expected datetime object value" );
            }
        }
    }
}
