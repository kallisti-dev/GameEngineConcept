using System;
using OpenTK.Graphics.OpenGL;

using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineConcept.Graphics
{
    public class TexturedVertexSet : VertexSet
    {
        public Texture Texture { get; protected set; }

        protected TexturedVertexSet() : base() { }

        public TexturedVertexSet(Texture tex, PrimitiveType mode, IAttributedVertexBuffer buffer, uint[] indices, int depth = 0, int[] enabledAttribs = null)
            : base(mode, buffer, indices, enabledAttribs)
        {
            Texture = Texture;
        }

        public override void Draw()
        {
            VertexBuffer.Bind(BufferTarget.ArrayBuffer, EnabledAttributes, (boundBuffer) =>
            {
                Texture.Bind(TextureUnit.Texture0, () =>
                {
                    boundBuffer.DrawElements(DrawMode, VertexBufferIndices);
                });
            });
        }
    }
}
