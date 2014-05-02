using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace GameEngineConcept.VertexBuffers
{
    class AttributedVertexBuffer : IAttributedVertexBuffer, IHasVertexBuffer<IBindableVertexBuffer>, IHasVertexAttributes
    {
        public IBindableVertexBuffer Buffer { get; protected set; }
        public VertexAttribute[] VertexAttributes { get; protected set; }

        public AttributedVertexBuffer(IBindableVertexBuffer buffer, VertexAttribute[] attributes)
        {
            Buffer = buffer;
            VertexAttributes = attributes;
        }

        public void LoadData<T>(BufferUsageHint hint, T[] data) where T : struct
        {
            Buffer.LoadData(hint, data);
        }

        public void Bind(BufferTarget target, IEnumerable<int> indices, Action<IBoundVertexBuffer> inner)
        {
            if (indices == null)
            {
                Bind(target, inner);
                return;
            }
            Buffer.Bind(target, (b) =>
            {
                b.WithAttributes(VertexAttributes, indices, () => inner(b));
            });
        }

        public void Bind(BufferTarget target, Action<IBoundVertexBuffer> inner)
        {
            Buffer.Bind(target, (b) =>
            {
                b.WithAttributes(VertexAttributes, () => inner(b));
            });
        }
    }
}
