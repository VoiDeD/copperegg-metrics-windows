using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CopperEggLib
{
    static class EnumerableEx
    {
        public static IEnumerable<T> FromSingle<T>( T item )
        {
            yield return item;
        }
    }

    static class ReflectionExtensions
    {
        public static T GetAttribute<T>( this MemberInfo memberInfo )
        {
            var attribs = memberInfo.GetCustomAttributes( typeof( T ), false );

            return attribs.Cast<T>().FirstOrDefault();
        }
    }
}
