using System;
using OpenTK.Graphics.OpenGL;
using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineConcept.Graphics
{
    public class VertexSet : IDrawable, IHasVertexAttributes
    {
        public IAttributedVertexBuffer vertexBuffer;
        public PrimitiveType drawMode;
        protected VertexIndices indices;
        protected int[] enabledAttributes;


        public VertexAttribute[] VertexAttributes { get { return vertexBuffer.VertexAttributes; } }

        protected VertexSet() { }

        public VertexSet(PrimitiveType mode, IAttributedVertexBuffer buffer, VertexIndices indices, int[] enabledAttribs = null)
        {
            drawMode = mode;
            vertexBuffer = buffer;
            enabledAttributes = enabledAttribs;
            this.indices = indices;
        }
         
        public virtual void Draw()
        {
            vertexBuffer.Bind(BufferTarget.ArrayBuffer, enabledAttributes, (boundBuffer) =>
            {
                boundBuffer.Draw(drawMode, indices);
            });
        }
    }
}
