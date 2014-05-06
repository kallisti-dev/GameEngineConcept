using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace GameEngineConcept.Graphics.VertexBuffers
{
    public class VertexBuffer : IRelease, IBindableVertexBuffer
    {
        [ThreadStaticAttribute]
        static VertexBuffer[] bindTable = new VertexBuffer[Enum.GetNames(typeof(BufferTarget)).Length];

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
            return b.CompareTo(vboId);
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
            if (bindTable[(uint)target] != this) //prevent unnecessary openGL calls
            {
                GL.BindBuffer(target, vboId);
                bindTable[(uint)target] = this;
            }
        }

        public void Bind(BufferTarget target, Action<IBoundVertexBuffer> handler)
        {
            VertexBuffer previousBind = bindTable[(uint)target];
            Bind(target);
            try { handler(new BoundVertexBuffer(this, target)); }
            finally
            {
                if (previousBind == null)
                    previousBind.Bind(target);
            }
        }

        public void Dispose()
        {
            EngineWindow.ReleaseOnMainThread(this);
            GC.SuppressFinalize(this);
        }
    }
}
