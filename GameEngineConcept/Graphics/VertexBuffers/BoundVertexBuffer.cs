using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GameEngineConcept.Graphics.VertexBuffers
{

    using VertexAttributes;

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

        public void WithAttributes(VertexAttributeSet attrs, Action inner)
        {
            EnableAttributes(attrs);
            try { inner(); }
            finally { DisableAttributes(attrs.Indices); }
        }

        public void WithAttributes(VertexAttributeSet attrs, IEnumerable<int> indices, Action inner)
        {
            EnableAttributes(attrs, indices);
            try { inner(); }
            finally { DisableAttributes(indices); }
        }


        private void EnableAttributes(VertexAttributeSet attrs)
        {
            foreach (var attr in attrs) {
                attr.Load();
            }
        }

        private void EnableAttributes(VertexAttributeSet attrs, IEnumerable<int> indices)
        {
            foreach(var attr in attrs) {
                attr.Load();
            }
            foreach(int i in indices) {
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
