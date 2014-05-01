using System.Collections;
using System.Collections.Generic;

namespace GameEngineConcept
{
    public class ComponentCollection : IComponentCollection
    {
        List<IComponent> components;

        /* Constructors */
        public ComponentCollection()
        {
            components = new List<IComponent>();
        }

        /* IEnumerable interface */
        public IEnumerator<IComponent> GetEnumerator() { return components.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return components.GetEnumerator(); }

        /* ICollection interface */
        public bool IsReadOnly { get { return false; } }
        public int Count { get { return components.Count; } }
        public bool Contains(IComponent c) { return components.Contains(c); }
        public void CopyTo(IComponent[] a, int ind) { components.CopyTo(a, ind); }
        public void Clear() { components.Clear(); }

        public void Add(IComponent c)
        {
            components.Add(c);
        }

        public bool Remove(IComponent c)
        {
            return components.Remove(c);
        }

        /* IComponent interface */
        public void Update()
        {
            foreach (IComponent c in components) { c.Update(); }
        }

        /* IComponentCollection interface */
        public void Update<C>() where C : IComponent
        {
            foreach (IComponent c in components)
            {
                IComponentCollection collection;
                if (c is C)
                {
                    c.Update();
                }
                else if ((collection = c as IComponentCollection) != null)
                {
                    collection.Update<C>();
                }
            }
        }
        public void Broadcast<T>(T obj)
        {
            foreach (IComponent c in components)
            {
                IReceiverComponent<T> rc;
                IComponentCollection coll;
                if ((coll = c as IComponentCollection) != null)
                {
                    coll.Broadcast<T>(obj);
                }
                else if ((rc = c as IReceiverComponent<T>) != null)
                {
                    rc.Receive(obj);
                }
            }
        }
    }
}
