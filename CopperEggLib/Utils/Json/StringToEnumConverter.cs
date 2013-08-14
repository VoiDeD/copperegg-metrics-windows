using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CopperEggLib
{
    class StringToEnumConverter : JsonConverter
    {
        public override bool CanConvert( Type objectType )
        {
            return objectType.IsEnum;
        }

        public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
        {
            if ( reader.TokenType != JsonToken.String )
                throw new JsonSerializationException( string.Format( "Unexpected token when parsing enum string. Expected string, got {0}", reader.TokenType ) );

            string jsonName = ( string )reader.Value;

            foreach ( var enumMember in objectType.GetFields( BindingFlags.Public | BindingFlags.Static ) )
            {
                string enumName = enumMember.Name;

                var attrib = enumMember.GetAttribute<EnumMemberAttribute>();

                if ( attrib != null )
                {
                    enumName = attrib.Value;
                }

                if ( string.Equals( enumName, jsonName, StringComparison.OrdinalIgnoreCase ) )
                    return enumMember.GetValue( null );
            }

            return existingValue;
        }

        public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
        {
            var objectType = value.GetType();

            if ( objectType.IsEnum )
            {
                var fields = objectType.GetFields( BindingFlags.Public | BindingFlags.Static );

                var matchedField = fields
                    .Where( f => string.Equals( f.Name, Enum.GetName( objectType, value ), StringComparison.OrdinalIgnoreCase ) )
                    .First();

                string jsonValue = matchedField.Name;


                var enumAttrib = matchedField.GetAttribute<EnumMemberAttribute>();

                if ( enumAttrib != null )
                    jsonValue = enumAttrib.Value;

                writer.WriteValue( jsonValue );
            }
            else
            {
                throw new JsonSerializationException( "Expected enum object value" );
            }
        }

        Dictionary<FieldInfo, string> GetEnumLookup( Type type )
        {
            return type.GetFields( BindingFlags.Public )
                .ToDictionary( f => f, f =>
                {
                    var name = f.Name;

                    var attrib = f.GetAttribute<EnumMemberAttribute>();

                    if ( attrib != null )
                        name = attrib.Value;

                    return name;
                } );
        }
    }
}
