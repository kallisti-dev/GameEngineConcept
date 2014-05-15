using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineConcept.Components
{
    //basic game component interface
    public interface IComponent
    {
        //update 1 frame
        void Update();
    }

    //a component that can receive an object of type T
    public interface IReceiver<in T> : IComponent
        where T : Message
    {
        void Receive(T obj);
    }

    //a collection of game components, which itself acts as a component
    public interface IComponentCollection : IComponent, ICollection<IComponent>
    {
    }

    //component collection extensions
    public static class IComponentCollectionExtensions
    {

        public static IEnumerable<C> Fetch<C>(this IEnumerable<IComponent> @this) where C : IComponent
        {
            foreach (var c in @this) {
                IComponentCollection coll;
                if (c is C)
                    yield return (C)c;
                if ((coll = c as IComponentCollection) != null) {
                    foreach (var c2 in coll.Fetch<C>()) {
                        yield return c2;
                    }
                }
            }
        }

        //Fetch all descendent components
        public static IEnumerable<IComponent> Fetch(this IEnumerable<IComponent> @this)
        {
            return @this.Fetch<IComponent>();
        }

        //Broadcast a message of type T to all descendents that implement IReceiver<T>
        public static void Broadcast<T>(this IEnumerable<IComponent> @this, T obj) 
            where T : Message
        {
            foreach (var c in @this.Fetch<IReceiver<T>>()) {
                c.Receive(obj);
            }
        }

        //Update all components whose type matchs C
        public static void Update<C>(this IEnumerable<IComponent> @this) 
            where C : IComponent
        {
            foreach (var c in @this.Fetch<C>()) {
                c.Update();
            }
        }

    }
}
