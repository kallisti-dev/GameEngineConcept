using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace GameEngineConcept.Buffers
{
    public class VertexBuffer : IDisposable, IVertexBufferBindable
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

        public void LoadData<T>(BufferUsageHint hint, T[] data) where T : struct
        {
            Bind(BufferTarget.ArrayBuffer, (b) => b.LoadData(hint, data));
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
                {
                    bindTable[(uint)target] = null;
                    GL.BindBuffer(target, 0);
                }
                else
                    previousBind.Bind(target);
            }
        }

        public void Dispose()
        {
            //TODO: batch multiple deletes into one DeleteBuffers call
            if (vboId != 0)
            {
                GL.DeleteBuffer(vboId);
                vboId = 0;
            }
            GC.SuppressFinalize(this);
        }
    }
}
