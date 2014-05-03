using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace GameEngineConcept.Graphics.VertexBuffers
{

    public interface IHasVertexAttributes
    {
        VertexAttribute[] VertexAttributes { get; }
    }

    public interface IHasVertexBuffer<B> where B : IVertexBuffer
    {
        B Buffer { get; }
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
        void WithAttributes(VertexAttribute[] attrs, Action inner);
        void WithAttributes(VertexAttribute[] attrs, IEnumerable<int> indices, Action inner);
    }
}
