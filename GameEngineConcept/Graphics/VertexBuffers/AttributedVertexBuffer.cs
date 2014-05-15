using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;

namespace GameEngineConcept.Graphics.VertexBuffers
{
    public class AttributedVertexBuffer : IAttributedVertexBuffer, IHasVertexBuffer<IBindableVertexBuffer>, IHasVertexAttributes
    {
        public IBindableVertexBuffer VBuffer { get; private set; }
        public VertexAttribute[] VertexAttributes { get; private set; }

        public AttributedVertexBuffer(IBindableVertexBuffer buffer, VertexAttribute[] attributes)
        {
            VBuffer = buffer;
            VertexAttributes = attributes;
        }

        public int CompareTo(int i)
        {
            return VBuffer.CompareTo(i);
        }

        public int CompareTo(IVertexBuffer b)
        {
            return VBuffer.CompareTo(b);
        }

        public void LoadData<T>(BufferUsageHint hint, T[] data) where T : struct
        {
            VBuffer.LoadData(hint, data);
        }

        public T[] GetData<T>(int offset, int size) where T : struct
        {
            return VBuffer.GetData<T>(offset, size);
        }

        public void SetData<T>(int offset, T[] data) where T : struct
        {
            VBuffer.SetData<T>(offset, data);
        }

        public void Bind(BufferTarget target, IEnumerable<int> enabledAttributes, Action<IBoundVertexBuffer> inner)
        {
            if (enabledAttributes == null)
            {
                Bind(target, inner);
                return;
            }
            VBuffer.Bind(target, (b) =>
            {
                b.WithAttributes(VertexAttributes, enabledAttributes, () => inner(b));
            });
        }

        public void Bind(BufferTarget target, Action<IBoundVertexBuffer> inner)
        {
            VBuffer.Bind(target, (b) =>
            {
                b.WithAttributes(VertexAttributes, () => inner(b));
            });
        }

        public int[] GetAttributeIndices() 
        {
            int[] indices = new int[VertexAttributes.Length];
            for (int i = 0; i < indices.Length; ++i)
                indices[i] = VertexAttributes[i].index;
            return indices;
        }
    }
}
