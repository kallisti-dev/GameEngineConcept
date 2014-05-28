using System;
using System.Collections.Generic;

namespace GameEngineConcept.Graphics
{

    //object that can draw with no extra parameters needed
    public interface IDrawable : IComparable<IDrawable>
    {
        void Draw();
        int DrawDepth { get; }
    }



    public interface IDrawableCollection : IDrawable, IReadOnlyCollection<IDrawable>
    {

    }
}
