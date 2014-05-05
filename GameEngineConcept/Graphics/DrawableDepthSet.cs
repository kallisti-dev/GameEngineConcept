using System;
using System.Collections;
using System.Collections.Generic;

namespace GameEngineConcept.Graphics
{
    [Serializable]
    public class DrawableDepthSet : SortedSet<IDrawableDepth>, IDrawable
    {
        static IComparer<IDrawableDepth> comparer = Comparer<IDrawableDepth>.Create((a, b) => a.DrawDepth.CompareTo(b.DrawDepth));

        public DrawableDepthSet() : base(comparer) { }

        public void Draw()
        {
            foreach(var obj in this) 
            {
                obj.Draw();
            }
        }
    }
}
