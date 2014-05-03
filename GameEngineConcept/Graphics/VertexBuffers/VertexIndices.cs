using System;
using OpenTK.Graphics.OpenGL;

namespace GameEngineConcept.Graphics.VertexBuffers
{

    //represents a collection of indices referring to vertices stored in a vertex buffer
    public abstract class VertexIndices
    {
        public static VertexIndices Create(uint[] indices)
        {
            return new UIntIndexArray(indices);
        }
        public static VertexIndices Create(ushort[] indices) 
        {
            return new UShortIndexArray(indices);
        }
        public static VertexIndices Create(byte[] indices) 
        {
            return new ByteIndexArray(indices);
        }
        public static VertexIndices Create(int first, int count) 
        {
            return new IndexRange(first, count);
        }
        public static VertexIndices Create(VertexBuffer indexBuffer, int count, DrawElementsType indexType) 
        {
            return new IndexBuffer(indexBuffer, count, indexType);
        }

        internal abstract void DrawVerticies(PrimitiveType mode);

    }

    public class UIntIndexArray : VertexIndices
    {
        uint[] array;

        public UIntIndexArray(uint[] indices) 
        {
            array = indices;
        }

        internal override void DrawVerticies(PrimitiveType mode) 
        {
            GL.DrawElements(mode, array.Length, DrawElementsType.UnsignedInt, array);
        }

    }

    public class UShortIndexArray : VertexIndices
    {
        ushort[] array;

        public UShortIndexArray(ushort[] indices)
        {
            array = indices;
        }

        internal override void DrawVerticies(PrimitiveType mode) 
        {
            GL.DrawElements(mode, array.Length, DrawElementsType.UnsignedShort, array);
        }
    }
 
    public class ByteIndexArray : VertexIndices
    {
        byte[] array;

        public ByteIndexArray(byte[] indices)
        {
            array = indices;
        }

        internal override void DrawVerticies(PrimitiveType mode)
        {
            GL.DrawElements(mode, array.Length, DrawElementsType.UnsignedByte, array);
        }
    }

    public class IndexRange : VertexIndices
    {
        int first;
        int count;

        public IndexRange(int first, int count)
        {
            this.first = first;
            this.count = count;
        }

        internal override void DrawVerticies(PrimitiveType mode)
        {
            GL.DrawArrays(mode, first, count);
        }
    }

    public class IndexBuffer : VertexIndices
    {
        VertexBuffer buffer;
        int count;
        DrawElementsType type;

        public IndexBuffer(VertexBuffer indexBuffer, int count, DrawElementsType type)
        {
            buffer = indexBuffer;
            this.count = count;
            this.type = type;
        }

        internal override void DrawVerticies(PrimitiveType mode)
        {
            var s = this;
            buffer.Bind(BufferTarget.ElementArrayBuffer,
                (b) => GL.DrawElements(mode, s.count, s.type, 0));
        }
    }
}
