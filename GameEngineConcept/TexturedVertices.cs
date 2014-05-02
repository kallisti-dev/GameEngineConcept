using System;
using OpenTK.Graphics.OpenGL;

using GameEngineConcept.Buffers;

namespace GameEngineConcept
{
    public class TexturedVertices : IHasVertexBufferIndices<IAttributedVertexBuffer>, IDrawable
    {
        public Texture Texture { get; protected set; }
        public IAttributedVertexBuffer Buffer { get; protected set; }
        public uint[] BufferIndices { get; protected set; }
        public int[] EnabledAttributes { get; protected set; }
        public PrimitiveType DrawMode { get; protected set; }

        protected TexturedVertices() { }

        public TexturedVertices(PrimitiveType mode, Texture tex, IAttributedVertexBuffer buffer, uint[] indices, int depth = 0, int[] enabledAttribs = null)
        {
            DrawMode = mode;
            Texture = Texture;
            Buffer = buffer;
            EnabledAttributes = enabledAttribs;
            BufferIndices = indices;
        }

        public virtual void Draw()
        {
            Buffer.Bind(BufferTarget.ArrayBuffer, EnabledAttributes, (boundBuffer) =>
            {
                Texture.Bind(TextureUnit.Texture0, () =>
                {
                    boundBuffer.DrawElements(DrawMode, BufferIndices);
                });
            });
        }
    }
}
