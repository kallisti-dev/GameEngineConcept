using System;
using System.Collections;
using System.Collections.Generic;

using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineConcept.Graphics
{
    [Serializable]
    public class DrawableDepthSet : SortedSet<IDrawableDepth>, IDrawableDepth
    {
       static IComparer<IDrawableDepth> comparer = Comparer<IDrawableDepth>.Create((a, b) => {
           var cmp = a.DrawDepth.CompareTo(b.DrawDepth);
           if (cmp == 0)
               return a.CompareTo(b);
           else
               return cmp;
       });

       public int DrawDepth { get; protected set; }

        public DrawableDepthSet(int depth = 0) : base(comparer) 
        {
            DrawDepth = depth;
        }

        public void Draw()
        {
            foreach(var obj in this) 
            {
                obj.Draw();
            }
        }

        public int CompareTo(IVertexBuffer b)
        {
            return -b.CompareTo(DrawDepth);
        }

        public int CompareTo(IDrawable d)
        {
            return 0;
        }
    }
}
