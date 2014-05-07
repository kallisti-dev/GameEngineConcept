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

    public interface IAnimationState<S, A, AS>
        where AS : IAnimationState<S, A, AS>, new()
    {
        int CurrentFrame { get; }
        int NextFrame { get; }

        void Initialize(IAnimator<S, A, AS> animator, A animation);
        void ToFrame(int n);
        bool Animate();
    }

    public interface IAnimator<S, A, AS>
        where AS : IAnimationState<S, A, AS>, new()
    {
        S Subject { get; }
        A Animation { get; }
        AS State { get; }

        void Forward(int n);
        void Reverse(int n);

        void ToFrame(int n);
        bool Animate();
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
