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

    public interface IAnimationState<Subject, Animation>
        where Animation : IAnimation<Subject, Animation>
    {
        int CurrentFrame { get; }
        int NextFrame { get; }

        void ToFrame(int n);
        bool Animate();
    }

    public interface IAnimation<Subject, Animation>
        where Animation : IAnimation<Subject, Animation>
    {
        IAnimationState<Subject, Animation> CreateState(IAnimator<Subject, Animation> animator);
    }

    public interface IAnimator<S, A>
        where A : IAnimation<S, A>
    {
        S Subject { get; }
        A Animation { get; }
        IAnimationState<S, A> State { get; }

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
