using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;

using GameEngineConcept.Graphics;
using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineConcept
{
    //extension methods for in-library interfaces
    public static class GameEngineExtensions
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

        //inverse comparators for some comparison methods defined in library
        public static int CompareTo(this int i, IVertexBuffer b)
        {
            return -b.CompareTo(i);
        }

        public static int CompareTo(this IVertexBuffer b, IDrawable d)
        {
            return -d.CompareTo(b);
        }
    }
}
