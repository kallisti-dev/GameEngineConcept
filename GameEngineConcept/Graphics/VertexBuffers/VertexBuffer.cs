using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL;

namespace GameEngineConcept.Graphics.VertexBuffers
{
    public class VertexBuffer : IBindableVertexBuffer
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
            try { handler(new BoundVertexBuffer(target)); }
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
    }
}
