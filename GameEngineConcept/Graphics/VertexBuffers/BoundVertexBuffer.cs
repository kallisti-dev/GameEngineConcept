using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;

namespace GameEngineConcept.Graphics.VertexBuffers
{
    public sealed class BoundVertexBuffer : IBoundVertexBuffer
    {
        private VertexBuffer buffer;
        private BufferTarget target;

        internal BoundVertexBuffer(VertexBuffer buffer, BufferTarget target)
        {
            this.buffer = buffer;
            this.target = target;
        }

        public void LoadData<T>(BufferUsageHint hint, T[] data) where T : struct
        {
            GL.BufferData<T>(target, new IntPtr(data.Length * Marshal.SizeOf(typeof(T))), data, hint);
        }

        public void DrawRange(PrimitiveType mode, int first, int count)
        {
            GL.DrawArrays(mode, first, count);
        }

        public void DrawElements(PrimitiveType mode, byte[] indices)
        {
            GL.DrawElements(mode, indices.Length, DrawElementsType.UnsignedByte, indices);
        }

        public void DrawElements(PrimitiveType mode, uint[] indices)
        {
            GL.DrawElements(mode, indices.Length, DrawElementsType.UnsignedInt, indices);
        }

        public void DrawElements(PrimitiveType mode, ushort[] indices)
        {
            GL.DrawElements(mode, indices.Length, DrawElementsType.UnsignedShort, indices);
        }

        public void DrawElements(PrimitiveType mode, VertexBuffer indexBuffer, int count, DrawElementsType type)
        {
            indexBuffer.Bind(BufferTarget.ElementArrayBuffer,
                (b) => GL.DrawElements(mode, count, type, 0));
        }

        private void EnableAttributes(VertexAttribute[] attrs)
        {
            for (int i = 0; i < attrs.Length; ++i)
            {
                GL.EnableVertexAttribArray(i);
                GL.VertexAttribPointer(i, attrs[i].nComponents, attrs[i].type, attrs[i].normalized, attrs[i].stride, attrs[i].offset);
            }
        }

        private void EnableAttributes(VertexAttribute[] attrs, IEnumerable<int> indices)
        {
            for (int i = 0; i < attrs.Length; ++i)
            {
                GL.VertexAttribPointer(i, attrs[i].nComponents, attrs[i].type, attrs[i].normalized, attrs[i].stride, attrs[i].offset);
            }
            foreach (uint i in indices)
            {
                GL.EnableVertexAttribArray(i);
            }
        }

        private void DisableAttributes(IEnumerable<int> indices)
        {
            foreach (uint i in indices)
            {
                GL.DisableVertexAttribArray(i);
            }
        }

        public void WithAttributes(VertexAttribute[] attrs, Action inner)
        {
            EnableAttributes(attrs);
            try { inner(); }
            finally { DisableAttributes(Enumerable.Range(0, attrs.Length)); }
        }

        public void WithAttributes(VertexAttribute[] attrs, IEnumerable<int> indices, Action inner)
        {
            EnableAttributes(attrs, indices);
            try { inner(); }
            finally { DisableAttributes(indices); }
        }
    }
}
