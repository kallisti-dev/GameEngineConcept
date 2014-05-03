using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;

using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineConcept.Graphics
{
    class VertexLoader<V> where V : struct 
    {

        IAttributedVertexBuffer buffer;
        DynamicArray<V> vList;
        List<Tuple<PrimitiveType, IndexRange, int[]>> iList;
        int currentIndex;
        BufferUsageHint hint;
        

        public VertexLoader(BufferUsageHint hint, IAttributedVertexBuffer buffer) 
        {
            this.buffer = buffer;
            vList = new DynamicArray<V>();
            iList = new List<Tuple<PrimitiveType, IndexRange, int[]>>();
            currentIndex = 0;
            this.hint = hint;
        }

        public void AddVertexSet(PrimitiveType mode, V[] verticies, int[] enableAttribs = null) 
        {
            for (int i = 0; i < verticies.Length; ++i)
            {
                vList.Add(verticies[i]);
            }
            iList.Add(Tuple.Create(
                mode, 
                new IndexRange(currentIndex, verticies.Length),
                enableAttribs));
            currentIndex += verticies.Length;
        }

        public IEnumerable<VertexSet> LoadBuffer()
        {
            buffer.LoadData(hint, vList.InternalArray);
            return iList.Select((tuple) => new VertexSet(tuple.Item1, buffer, tuple.Item2, tuple.Item3));
        }


    }
}
