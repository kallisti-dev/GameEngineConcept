using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;

using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineConcept.Graphics.Loaders
{
    public abstract class BaseVertexLoader<BufferType, VIn, VState, VOut>
        where BufferType : IVertexBuffer
        where VIn : struct
    {

        protected BufferType buffer;
        protected int currentIndex;
        private BufferUsageHint hint;
        private DynamicArray<VIn> vList;
        private List<VState> stateList;

        protected abstract VOut CreateVertexOutput(VState state);

        public BaseVertexLoader(BufferUsageHint hint, BufferType buffer)
        {
            this.buffer = buffer;
            vList = new DynamicArray<VIn>();
            stateList = new List<VState>();
            currentIndex = 0;
            this.hint = hint;
        }

        protected void AddVerticies(VIn[] verticies)
        {
            for (int i = 0; i < verticies.Length; ++i)
            {
                vList.Add(verticies[i]);
            }
            currentIndex += verticies.Length;
        }

        protected void AddState(VState s)
        {
            stateList.Add(s);
        }


        public IEnumerable<VOut> LoadBuffer()
        {
            buffer.LoadData(hint, vList.InternalArray);
            return stateList.Select(CreateVertexOutput);
        }
    }
}
