using OpenTK;
using System.Drawing;
using System.Runtime.InteropServices;


namespace GameEngineConcept.Graphics
{

    using VertexAttributes;

    [VertexStruct]
    [StructLayout(LayoutKind.Sequential)]
    public struct TexturedVertex2 : IHasVertexAttributes
    {
        public VertexAttributeSet VertexAttributes { get { return VertexAttributeSet.FromType<TexturedVertex2>(); } }

        [NormalizeComponents(true)]
        public Vector2 position;

        [NormalizeComponents(false)]
        public Point texel;

        public TexturedVertex2(Vector2 pos, Point tex) {
            position = pos;
            texel = tex;
        }

        public TexturedVertex2(float posX, float posY, int texX, int texY)
        {
            position = new Vector2(posX, posY);
            texel    = new Point(texX, texY);
        }
    }
}
