using System;
using OpenTK.Graphics.OpenGL;

using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineConcept.Graphics.Loaders
{
    public class VertexLoader<V> : AbstractVertexLoader<IAttributedVertexBuffer, V, VertexLoader<V>.State, VertexSet>
        where V : struct
    {
        public class State 
        {
            public PrimitiveType mode;
            public IndexRange indices;
            public int[] enabledAttribs;
        }

        public VertexLoader(BufferUsageHint hint, IAttributedVertexBuffer buffer) : base(hint, buffer) { }

        public void Add(PrimitiveType mode, V[] verticies, int[] enabledAttribs = null)
        {
            AddState(new State
            {
                mode = mode,
                indices = new IndexRange(currentIndex, verticies.Length),
                enabledAttribs = enabledAttribs
            });
            AddVerticies(verticies);
        }

        protected override VertexSet CreateVertexOutput(State s) 
        {
            return new VertexSet(s.mode, buffer, s.indices, s.enabledAttribs);
        }
    }
}
