using OpenTK.Graphics.OpenGL4;

using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineConcept.Graphics
{
    public class TexturedVertexSet : VertexSet
    {
        public Texture Texture { get; set; }

        protected TexturedVertexSet() : base() { }

        public TexturedVertexSet(Texture tex, PrimitiveType mode, IAttributedVertexBuffer buffer, VertexIndices indices, int[] enabledAttribs = null)
            : base(mode, buffer, indices, enabledAttribs)
        {
            Texture = tex;
        }

        public override void Draw()
        {
            VBuffer.Bind(BufferTarget.ArrayBuffer, EnabledAttributes, (boundBuffer) =>
            {
                Texture.Bind(TextureUnit.Texture0, () =>
                {
                    boundBuffer.Draw(DrawMode, indices);
                });
            });
        }
    }
}
