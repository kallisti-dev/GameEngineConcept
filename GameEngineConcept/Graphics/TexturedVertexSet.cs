using System;
using OpenTK.Graphics.OpenGL;

using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineConcept.Graphics
{
    public class TexturedVertexSet : VertexSet
    {
        public Texture texture;

        protected TexturedVertexSet() : base() { }

        public TexturedVertexSet(Texture tex, PrimitiveType mode, IAttributedVertexBuffer buffer, VertexIndices indices, int[] enabledAttribs = null)
            : base(mode, buffer, indices, enabledAttribs)
        {
            texture = tex;
        }

        public override void Draw()
        {
            VBuffer.Bind(BufferTarget.ArrayBuffer, EnabledAttributes, (boundBuffer) =>
            {
                texture.Bind(TextureUnit.Texture0, () =>
                {
                    boundBuffer.Draw(DrawMode, indices);
                });
            });
        }
    }
}
