using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngineConcept
{
    public static class IEnumerableExtensions
    {
        
        //a more efficient Reverse<T>() method for IList<T>
        public static IEnumerable<T> Reverse<T>(this IList<T> list)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                yield return list[i];
            }
        }

        //flattens nested IEnumerables
        public static IEnumerable<J> Join<J>(this IEnumerable<IEnumerable<J>> @this)
        {
            foreach (var js in @this)
                foreach (var j in js)
                    yield return j;
        }

    }
}
