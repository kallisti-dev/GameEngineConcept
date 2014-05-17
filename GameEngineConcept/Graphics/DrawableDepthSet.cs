using System;
using System.Collections.Generic;

namespace GameEngineConcept.Graphics
{

    [Serializable]
    public class DrawableDepthSet : SortedSet<IDrawable>, IDrawable
    {
       static IComparer<IDrawable> comparer = Comparer<IDrawable>.Create((a, b) => {
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

        public DrawableDepthSet(IEnumerable<IDrawable> e, int depth = 0) : base(e, comparer)
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

        public int CompareTo(IDrawable d)
        {
            return DrawDepth.CompareTo(d.DrawDepth);
        }
    }
}
