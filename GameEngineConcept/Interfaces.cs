using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

using GameEngineConcept.Buffers;

namespace GameEngineConcept
{
    //object that can draw with no extra parameters needed
    public interface IDrawable
    {
        void Draw();
    }

    //basic game component interface
    public interface IComponent
    {
        //update 1 frame
        void Update();
    }

    //a component that can receive an object of type T
    public interface IReceiverComponent<T> : IComponent
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

    public interface IHasVertexBuffer<B> where B : IVertexBuffer {
        B Buffer { get; }
    }

    public interface IHasVertexBufferIndices<B> : IHasVertexBuffer<B> where B : IVertexBuffer 
    {
        uint[] BufferIndices { get; }
    }
}
