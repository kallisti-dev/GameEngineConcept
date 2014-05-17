using System;
using System.Collections;
using System.Collections.Generic;

namespace GameEngineConcept.Util
{
    //since List<T> doesn't expose its internal array pointer, we need to roll our own dynamically resizing array implementation.
    public class DynamicArray<T> : IEnumerable<T>
    {

        const int defaultCapacity = 10;
        static Func<int, int> defaultResizer = (n) => n == 0 ? defaultCapacity : (2 * n);

        T[] array;
        Func<int, int> resizer;

        public int Count { get; protected set; }
        public T[] InternalArray {get { return array; }}

        T this[int i]
        {
            get { return array[i]; }
            set { array[i] = value; }
        }

        public DynamicArray(int initialCapacity = defaultCapacity, Func<int, int> resizer = null)
        {
            array = new T[initialCapacity];
            Count = initialCapacity;
            this.resizer = resizer ?? defaultResizer;
        }

        public void Add(T element)
        {
            if (Count == array.Length)
            {
                Array.Resize(ref array, resizer(array.Length));
            }

            array[Count++] = element;
        }

        public IEnumerator<T> GetEnumerator() { return ((IEnumerable<T>)array).GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return array.GetEnumerator(); }

    }
}
