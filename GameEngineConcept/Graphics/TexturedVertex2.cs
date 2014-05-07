using System;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineConcept.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TexturedVertex2 : IHasVertexAttributes
    {

        public static VertexAttribute[] vAttributes;

        //static constructor
        static TexturedVertex2() {
            Type t = typeof(TexturedVertex2);
            int size = Marshal.SizeOf(t);
            vAttributes = new[] {
                new VertexAttribute(0, 2, VertexAttribPointerType.Float, false, size, (int)Marshal.OffsetOf(t, "position")),
                new VertexAttribute(1, 2, VertexAttribPointerType.Int, false, size, (int)Marshal.OffsetOf(t, "texel"))
            };
        }

        public VertexAttribute[] VertexAttributes { get { return vAttributes; } }

        public Vector2 position;
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
