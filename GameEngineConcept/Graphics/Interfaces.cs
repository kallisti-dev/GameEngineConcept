using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace GameEngineConcept.Graphics
{
    using VertexBuffers;

    //object that can draw with no extra parameters needed
    public interface IDrawable : IComparable<IDrawable>, IComparable<IVertexBuffer>
    {
        void Draw();
    }

    public interface IDrawableDepth : IDrawable
    {
        int DrawDepth { get; }
    }

    public interface IHasDimensions<T> where T : struct
    {
        T Width { get; }
        T Height { get; }
    }

    public interface IHasPosition<T> where T : struct
    {
        T Position { get; set; }
    }

    //interface for objects with external openGL resources.
    public interface IRelease : IDisposable
    {
        void Release();
    }
}
