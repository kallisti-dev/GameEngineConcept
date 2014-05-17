using GameEngineConcept.Graphics.VertexBuffers;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace GameEngineConcept.Graphics.Loaders
{
    public class VertexLoader<V> : BaseVertexLoader<IAttributedVertexBuffer, V, VertexSet>
        where V : struct
    {
        public VertexLoader(BufferUsageHint hint, IAttributedVertexBuffer buffer) : base(hint, buffer) { }

        public void AddVertexSet(PrimitiveType mode, V[] vertices, ISet<int> enabledAttribs = null)
        {
            AddState(new VertexSet(mode, VBuffer, VertexIndices.FromRange(currentIndex, vertices.Length), enabledAttribs));
            AddVertices(vertices);
        }
    }
}
