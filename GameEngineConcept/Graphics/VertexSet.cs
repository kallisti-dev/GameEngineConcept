using System;
using OpenTK.Graphics.OpenGL;
using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineConcept.Graphics
{
    public class VertexSet : IDrawable, IHasVertexAttributes
    {
        public IAttributedVertexBuffer VertexBuffer { get; protected set; }
        public uint[] VertexBufferIndices { get; protected set; }
        public int[] EnabledAttributes { get; protected set; }
        public PrimitiveType DrawMode { get; protected set; }

        public VertexAttribute[] VertexAttributes { get { return VertexBuffer.VertexAttributes; } }

        protected VertexSet() { }

        public VertexSet(PrimitiveType mode, IAttributedVertexBuffer buffer, uint[] indices, int[] enabledAttribs = null)
        {
            DrawMode = mode;
            VertexBuffer = buffer;
            EnabledAttributes = enabledAttribs;
            VertexBufferIndices = indices;
        }

        public virtual void Draw()
        {
            VertexBuffer.Bind(BufferTarget.ArrayBuffer, EnabledAttributes, (boundBuffer) =>
            {
                boundBuffer.DrawElements(DrawMode, VertexBufferIndices);
            });
        }
    }
}
