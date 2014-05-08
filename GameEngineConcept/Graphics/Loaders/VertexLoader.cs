using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;

using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineConcept.Graphics.Loaders
{
    public class VertexLoader<V> : BaseVertexLoader<IAttributedVertexBuffer, V, VertexLoader<V>.State, VertexSet>
        where V : struct
    {
        public class State 
        {
            public PrimitiveType mode;
            public VertexIndices indices;
            public int[] enabledAttribs;
        }

        public VertexLoader(BufferUsageHint hint, IAttributedVertexBuffer buffer) : base(hint, buffer) { }

        public void AddVertexSet(PrimitiveType mode, V[] vertices, int[] enabledAttribs = null)
        {
            AddState(new State
            {
                mode = mode,
                indices = VertexIndices.Create(currentIndex, vertices.Length),
                enabledAttribs = enabledAttribs
            });
            AddVertices(vertices);
        }

        protected override VertexSet CreateVertexOutput(State s) 
        {
            return new VertexSet(s.mode, VBuffer, s.indices, s.enabledAttribs);
        }
    }
}
