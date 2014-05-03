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
    public interface IReceiverComponent<in T> : IComponent
    {
        void Receive(T obj);
    }

    //a collection of game components, which itself acts as a component
    public interface IComponentCollection : IComponent, ICollection<IComponent>
    {
        //broadcasts a state of type T to all inner components that implement IReceiverComponent<T>
        void Broadcast<T>(T state);
        //update all inner components that match type C
        void Update<C>() where C : IComponent;
    }
}
