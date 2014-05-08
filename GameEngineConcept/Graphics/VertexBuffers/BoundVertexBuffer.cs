using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;

namespace GameEngineConcept.Graphics.VertexBuffers
{
    public struct BoundVertexBuffer : IBoundVertexBuffer
    {
        IVertexBuffer buffer;
        BufferTarget target;

        internal BoundVertexBuffer(IVertexBuffer buffer, BufferTarget target)
        {
            this.buffer = buffer;
            this.target = target;
        }

        public int CompareTo(int i)
        {
            return buffer.CompareTo(i);
        }

        public int CompareTo(IVertexBuffer b)
        {
            return buffer.CompareTo(b);
        }

        public void LoadData<T>(BufferUsageHint hint, T[] data) where T : struct
        {
            GL.BufferData<T>(target, new IntPtr(data.Length * Marshal.SizeOf(typeof(T))), data, hint);
        }

        public T[] GetData<T>(int offset, int nElements) where T : struct
        {
            T[] @out = new T[nElements];
            GL.GetBufferSubData(target, new IntPtr(offset), new IntPtr(nElements * Marshal.SizeOf(typeof(T))), @out);
            return @out;
        }

        public void SetData<T>(int offset, T[] data) where T : struct
        {
            GL.BufferSubData(target, new IntPtr(offset), new IntPtr(data.Length * Marshal.SizeOf(typeof(T))), data);
        }

        public void Draw(PrimitiveType type, VertexIndices indices)
        {
            indices.DrawVertices(type);
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
    }
}
