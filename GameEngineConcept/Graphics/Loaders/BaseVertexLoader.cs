using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;

using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineConcept.Graphics.Loaders
{
    public abstract class BaseVertexLoader<BufferType, VIn, VState, VOut> : IHasVertexBuffer<BufferType>
        where BufferType : IVertexBuffer
        where VIn : struct
    {

        public BufferType VBuffer {get; set;}
        protected int currentIndex;
        private BufferUsageHint hint;
        private DynamicArray<VIn> vList;
        private List<VState> stateList;

        protected abstract VOut CreateVertexOutput(VState state);

        public virtual IEnumerable<VOut> Load()
        {
            LoadBuffer();
            return ConsumeAddedStates();
        }

        protected BaseVertexLoader(BufferUsageHint hint, BufferType buffer)
        {
            this.VBuffer = buffer;
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


        protected IEnumerable<VOut> ConsumeAddedStates()
        {
            var outs = stateList.Select(CreateVertexOutput);
            stateList.Clear();
            return outs;
        }

        protected void LoadBuffer()
        {
            VBuffer.LoadData(hint, vList.InternalArray);
        }
    }
}
