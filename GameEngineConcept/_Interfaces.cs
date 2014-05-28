using OpenTK;
using System;
using System.Drawing;

namespace GameEngineConcept
{
    public interface IHasDimensions<T> where T : struct
    {
        T Width { get; }
        T Height { get; }
    }

    public interface IHasPosition<T> where T : struct
    {
        T Position { get; set; }
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

    //interface for objects with external resources to deallocate.
    public interface IRelease : IDisposable
    {
        void Release();
    }

    public interface ISceneSnapshot
    {
        void Restore();
    }
}
