using System;
using System.Collections;
using System.Collections.Generic;

namespace GameEngineConcept.Graphics
{
    public class DepthComparer : IComparer<IDrawableDepth> 
    {
        public int Compare(IDrawableDepth a, IDrawableDepth b) 
        {
            return a.DrawDepth.CompareTo(b.DrawDepth);
        }
    }

    public class DepthSet : SortedSet<IDrawableDepth>, IDrawable
    {
        static DepthComparer comparer = new DepthComparer();

        public DepthSet() : base(comparer) { }

        public void Draw()
        {
            foreach(var obj in this) 
            {
                obj.Draw();
            }
        }
    }
}
