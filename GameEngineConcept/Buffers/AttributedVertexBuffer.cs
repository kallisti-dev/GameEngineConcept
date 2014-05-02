using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace GameEngineConcept
{
    class AttributedVertexBuffer : IVertexBufferLoadable
    {
        VertexBuffer buff;
        VertexAttribute[] attrs;

        public AttributedVertexBuffer(VertexBuffer buffer, VertexAttribute[] attributes)
        {
            buff = buffer;
            attrs = attributes;
        }

        public void LoadData<T>(BufferUsageHint hint, T[] data) where T : struct
        {
            buff.LoadData(hint, data);
        }

        public void Bind(BufferTarget target, IEnumerable<int> indices, Action<BoundVertexBuffer> inner)
        {
            buff.Bind(target, (b) =>
            {
                b.WithAttributes(attrs, indices, () => inner(b));
            });
        }

        public void Bind(BufferTarget target, Action<BoundVertexBuffer> inner)
        {
            buff.Bind(target, (b) =>
            {
                b.WithAttributes(attrs, () => inner(b));
            });
        }
    }
}
