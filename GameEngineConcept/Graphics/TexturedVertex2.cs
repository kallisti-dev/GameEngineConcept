using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;

using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineConcept.Graphics
{
    public struct TexturedVertex2 : IHasVertexAttributes
    {

        public static VertexAttribute[] vAttributes;

        //static constructor
        static TexturedVertex2() {
            Type t = typeof(TexturedVertex2);
            int size = Marshal.SizeOf(t);
            vAttributes = new[] {
                new VertexAttribute(0, 2, VertexAttribPointerType.Float, false, size, (int)Marshal.OffsetOf(t, "position")),
                new VertexAttribute(1, 2, VertexAttribPointerType.Float, false, size, (int)Marshal.OffsetOf(t, "texel"))
            };
        }

        public VertexAttribute[] VertexAttributes { get { return vAttributes; } }

        public Vector2 position;
        public Vector2 texel;

        public TexturedVertex2(Vector2 pos, Vector2 tex) {
            position = pos;
            texel = tex;
        }

        public TexturedVertex2(float posX, float posY, float texX, float texY)
        {
            position = new Vector2(posX, posY);
            texel    = new Vector2(texX, texY);
        }
    }
}
