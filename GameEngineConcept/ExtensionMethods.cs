using System;
using System.Collections.Generic;

namespace GameEngineConcept
{
    //extension methods to external classes
    public static class ExtensionMethods
    {
        //a more efficient Reverse<T>() method for IList<T>
        public static IEnumerable<T> Reverse<T>(this IList<T> list)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                yield return list[i];
            }
        }

        public static IEnumerable<J> Join<J>(this IEnumerable<IEnumerable<J>> @this)
        {
            foreach (var js in @this)
                foreach (var j in js)
                    yield return j;
        }
    }
}
