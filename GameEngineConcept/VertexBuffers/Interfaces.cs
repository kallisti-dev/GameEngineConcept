using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace GameEngineConcept.VertexBuffers
{

    public interface IHasVertexBuffer<B> where B : IVertexBuffer
    {
        B Buffer { get; }
    }

    public interface IHasVertexBufferIndices<B> : IHasVertexBuffer<B> where B : IVertexBuffer
    {
        uint[] BufferIndices { get; }
    }

    public interface IHasVertexAttributes
    {
        VertexAttribute[] VertexAttributes { get; }
    }

    public interface IVertexBuffer
    {
        void LoadData<T>(BufferUsageHint hint, T[] data) where T : struct;
    }

    public interface IDrawableVertexBuffer : IVertexBuffer
    {
        void DrawElements(PrimitiveType mode, VertexBuffer indexBuffer, int count, DrawElementsType type);
        void DrawElements(PrimitiveType mode, byte[] indices);
        void DrawElements(PrimitiveType mode, ushort[] indices);
        void DrawElements(PrimitiveType mode, uint[] indices);
        void DrawRange(PrimitiveType mode, int first, int count);
    }

    public interface IAttributableVertexBuffer : IVertexBuffer
    {
        void WithAttributes(VertexAttribute[] attrs, Action inner);
        void WithAttributes(VertexAttribute[] attrs, IEnumerable<int> indices, Action inner);
    }

    public interface IBindableVertexBuffer : IVertexBuffer
    {
        void Bind(BufferTarget target, Action<IBoundVertexBuffer> handler);
    }

    public interface IAttributedVertexBuffer : IHasVertexAttributes, IBindableVertexBuffer
    {
        void Bind(BufferTarget target, IEnumerable<int> indices, Action<IBoundVertexBuffer> inner);
    }

    public interface IBoundVertexBuffer : IDrawableVertexBuffer, IAttributableVertexBuffer
    {

    }
}
