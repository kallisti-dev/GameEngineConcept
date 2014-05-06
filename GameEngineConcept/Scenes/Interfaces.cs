using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineConcept.Scenes
{
    //TODO: progress meters?
    public interface IScene
    {
        bool IsLoaded { get; }

        Task Load(VertexBufferPool pool);

        void Unload(VertexBufferPool pool);

        void Activate(EngineWindow window);

        void Deactivate(EngineWindow window);

    }
}
