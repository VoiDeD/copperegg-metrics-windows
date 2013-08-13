using System;
using System.Collections.Generic;
using System.Linq;
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
}
