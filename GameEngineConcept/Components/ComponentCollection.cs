using System.Collections;
using System.Collections.Generic;

namespace GameEngineConcept.Components
{
    public class ComponentCollection : HashSet<IComponent>, IComponentCollection
    {
        public void Update()
        {
            foreach (IComponent c in this) { c.Update(); }
        }

        /* IComponentCollection interface */
        public void Update<C>() where C : IComponent
        {
            foreach (IComponent c in this)
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
        public void Broadcast<T>(T obj) where T : Message
        {
            foreach (IComponent c in this)
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
