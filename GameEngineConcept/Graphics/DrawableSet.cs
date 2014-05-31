using System;
using System.Collections.Generic;

namespace GameEngineConcept.Graphics
{

    [Serializable]
    public class DrawableSet : SortedSet<IDrawable>, IDrawableCollection
    {
       /* 
        //deprecated
       static IComparer<IDrawable> comparer = Comparer<IDrawable>.Create((a, b) => {
           var cmp = a.DrawDepth.CompareTo(b.DrawDepth);
           if (cmp == 0)
               return a.CompareTo(b);
           else
               return cmp;
       });
        */

        public int DrawDepth { get { return 0; } }

        public DrawableSet() : base() { }
        public DrawableSet(IEnumerable<IDrawable> e) : base(e) { }

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
