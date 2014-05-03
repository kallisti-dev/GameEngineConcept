using System;
using OpenTK.Graphics.OpenGL;
using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineConcept.Graphics
{
    public class VertexSet : IDrawable, IHasVertexBuffer<IAttributedVertexBuffer>, IHasVertexAttributes
    {
        public IAttributedVertexBuffer VBuffer { get; protected set; }
        public PrimitiveType DrawMode { get; protected set; }
        protected VertexIndices indices;
        protected int[] enabledAttributes;


        public VertexAttribute[] VertexAttributes { get { return VBuffer.VertexAttributes; } }

        protected VertexSet() { }

        public VertexSet(PrimitiveType mode, IAttributedVertexBuffer buffer, VertexIndices indices, int[] enabledAttribs = null)
        {
            DrawMode = mode;
            VBuffer = buffer;
            enabledAttributes = enabledAttribs;
            this.indices = indices;
        }
         
        public virtual void Draw()
        {
            VBuffer.Bind(BufferTarget.ArrayBuffer, enabledAttributes, (boundBuffer) =>
            {
                boundBuffer.Draw(DrawMode, indices);
            });
        }
    }
}
