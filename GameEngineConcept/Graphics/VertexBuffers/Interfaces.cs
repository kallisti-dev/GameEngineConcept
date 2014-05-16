using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;

namespace GameEngineConcept.Graphics.VertexBuffers
{
    using VertexAttributes;

    public interface IHasVertexBuffer<out B> where B : IVertexBuffer
    {
        B VBuffer { get; }
    }

    public interface IVertexBuffer : IComparable<int>, IComparable<IVertexBuffer>
    {
        void LoadData<T>(BufferUsageHint hint, T[] data) where T : struct;
        T[] GetData<T>(int offset, int nElements) where T : struct;
        void SetData<T>(int offset, T[] data) where T : struct;
    }

    public interface IDrawableVertexBuffer : IVertexBuffer
    {
        void Draw(PrimitiveType type, VertexIndices indices);
    }

    public interface IBindableVertexBuffer : IVertexBuffer
    {
        void Bind(BufferTarget target, Action<IBoundVertexBuffer> handler);
    }

    public interface IAttributedVertexBuffer : IHasVertexAttributes, IBindableVertexBuffer
    {
        void Bind(BufferTarget target, IEnumerable<int> enabledAttributes, Action<IBoundVertexBuffer> inner);
    }

    public interface IBoundVertexBuffer : IDrawableVertexBuffer
    {
        void WithAttributes(VertexAttributeSet attrs, Action inner);
        void WithAttributes(VertexAttributeSet attrs, IEnumerable<int> indices, Action inner);
    }
}
