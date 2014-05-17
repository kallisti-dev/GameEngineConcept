using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngineConcept.Graphics.VertexBuffers
{
    public class VertexBuffer : IRelease, IBindableVertexBuffer
    {

        [ThreadStaticAttribute]
        static BindData[] bindTable = new BindData[Enum.GetNames(typeof(BufferTarget)).Length];

        private struct BindData
        {
            public VertexBuffer buffer;
            public int depth;
        }

        int vboId;

        protected VertexBuffer(int id)
        {
            vboId = id;
        }

        public static VertexBuffer Allocate()
        {
            return new VertexBuffer(GL.GenBuffer());
        }

        public static IEnumerable<VertexBuffer> Allocate(int n)
        {
            int[] vboIds = new int[n];
            GL.GenBuffers(n, vboIds);
            return vboIds.Select((id) => new VertexBuffer(id));
        }

        public static IEnumerable<VertexBuffer> Allocate(uint n)
        {
            return Allocate(Convert.ToInt32(n));
        }

        public int CompareTo(int i)
        {
            return vboId.CompareTo(i);
        }

        public int CompareTo(IVertexBuffer b)
        {
            return -b.CompareTo(vboId);
        }

        public void LoadData<T>(BufferUsageHint hint, T[] data) where T : struct
        {
            Bind(BufferTarget.ArrayBuffer, (b) => b.LoadData(hint, data));
        }

        public T[] GetData<T>(int offset, int size) where T : struct
        {
            T[] @out = null;
            Bind(BufferTarget.ArrayBuffer, (b) => { @out = b.GetData<T>(offset, size); });
            return @out;
        }

        public void SetData<T>(int offset, T[] data) where T : struct
        {
            Bind(BufferTarget.ArrayBuffer, (b) => b.SetData(offset, data));
        }

        public void Release() 
        {
            GL.DeleteBuffer(vboId);
        }

        private void Bind(BufferTarget target)
        {
            uint t = (uint)target;
            if (bindTable[t].buffer != this) //prevent unnecessary openGL calls
            {
                GL.BindBuffer(target, vboId);
                bindTable[t].buffer = this;
            }
        }

        public void Bind(BufferTarget target, Action<IBoundVertexBuffer> handler)
        {
            uint t = (uint) target;
            BindData previousBind = bindTable[t];
            Bind(target);
            bindTable[t].depth++;
            try { handler(new BoundVertexBuffer(this, target)); }
            finally
            {
                if (previousBind.depth > 0)
                    previousBind.buffer.Bind(target);
                bindTable[t].depth--;
            }
        }

        public void Dispose()
        {
            EngineWindow.ReleaseOnMainThread(this);
            GC.SuppressFinalize(this);
        }
    }
}
