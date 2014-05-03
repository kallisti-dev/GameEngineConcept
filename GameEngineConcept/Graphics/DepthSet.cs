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

    public class DepthSet : IDrawable, ICollection<IDrawableDepth>
    {
        SortedSet<IDrawableDepth> depthSet;

        public bool IsReadOnly { get { return true; } }
        public int Count { get { return depthSet.Count; } }

        public DepthSet() {
            depthSet = new SortedSet<IDrawableDepth>(new DepthComparer());
        }

        public void Add(IDrawableDepth obj)
        {
            depthSet.Add(obj);
        }

        public bool Remove(IDrawableDepth obj)
        {
            return depthSet.Remove(obj);
        }

        public bool Contains(IDrawableDepth obj)
        {
            return depthSet.Contains(obj);
        }

        public void CopyTo(IDrawableDepth[] arr, int index) {
            depthSet.CopyTo(arr, index);
        }

        public void Clear()
        {
            depthSet.Clear();
        }

        public void Draw()
        {
            foreach(var obj in depthSet) 
            {
                obj.Draw();
            }
        }

        public void Resort()
        {
            depthSet = new SortedSet<IDrawableDepth>(depthSet);
        }

        public IEnumerator<IDrawableDepth> GetEnumerator() { return depthSet.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return depthSet.GetEnumerator(); }
    }
}
