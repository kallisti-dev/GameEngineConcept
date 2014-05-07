using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace GameEngineConcept.Graphics.VertexBuffers
{

    public interface IHasVertexAttributes
    {
        VertexAttribute[] VertexAttributes { get; }
    }

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
        int[] GetAttributeIndices();
    }

    public interface IBoundVertexBuffer : IDrawableVertexBuffer
    {
        void WithAttributes(VertexAttribute[] attrs, Action inner);
        void WithAttributes(VertexAttribute[] attrs, IEnumerable<int> indices, Action inner);
    }
}
