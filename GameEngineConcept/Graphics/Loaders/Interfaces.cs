using System;
using System.Collections.Generic;


namespace GameEngineConcept.Graphics.Loaders
{
    interface ILoader<T>
    {
        IEnumerable<T> Load();
    }
}
