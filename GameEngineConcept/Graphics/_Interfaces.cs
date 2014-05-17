using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameEngineConcept.Graphics
{

    //object that can draw with no extra parameters needed
    public interface IDrawable : IComparable<IDrawable>
    {
        void Draw();
        int DrawDepth { get; }
    }

    public interface IHasDimensions<T> where T : struct
    {
        T Width { get; }
        T Height { get; }
    }

    public interface IDrawableCollection : IDrawable, IReadOnlyCollection<IDrawable>
    {

    }

    public static class IHasDimensionsExtensions
    {
        //get Size/Vector2 from dimension-having objects
        public static Size GetSize(this IHasDimensions<int> @this)
        {
            return new Size(@this.Width, @this.Height);
        }

        public static Vector2 GetSize(this IHasDimensions<float> @this)
        {
            return new Vector2(@this.Width, @this.Height);
        }
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
