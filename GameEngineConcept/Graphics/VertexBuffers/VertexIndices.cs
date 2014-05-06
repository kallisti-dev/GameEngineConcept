using System;
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace GameEngineConcept.Graphics.VertexBuffers
{

    //represents a collection of indices referring to vertices stored in a vertex buffer
    public sealed class VertexIndices
    {
        Action<PrimitiveType> drawFunc;

        private VertexIndices(Action<PrimitiveType> f) { drawFunc = f; }

        internal void DrawVerticies(PrimitiveType mode)
        {
            drawFunc(mode);
        }

        public static VertexIndices Create(uint[] indices)
        {
            return new VertexIndices((t) => 
                GL.DrawElements(t, indices.Length, DrawElementsType.UnsignedInt, indices));
        }
        public static VertexIndices Create(ushort[] indices) 
        {
            return new VertexIndices((t) => 
                GL.DrawElements(t, indices.Length, DrawElementsType.UnsignedShort, indices));
        }
        public static VertexIndices Create(byte[] indices) 
        {
            return new VertexIndices((t) => 
                GL.DrawElements(t, indices.Length, DrawElementsType.UnsignedByte, indices));
        }
        public static VertexIndices Create(int first, int count) 
        {
            return Create(Enumerable.Range(first, count).Select(Convert.ToUInt32).ToArray());
            //Old implementation uses GL.DrawArrays, but GL.DrawElements exhibits better cache performance
            //return new VertexIndices((t) => GL.DrawArrays(t, first, count));
        }
        public static VertexIndices Create(VertexBuffer indexBuffer, int count, DrawElementsType indexType)
        {
            return new VertexIndices((t) =>
            {
                indexBuffer.Bind(BufferTarget.ElementArrayBuffer, (b) =>
                    GL.DrawElements(t, count, indexType, 0));
            });
        }
    }
}
