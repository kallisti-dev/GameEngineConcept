using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineConcept.Scenes
{
    public interface IScene
    {
        bool IsLoaded { get; }

        void Load();

        void Unload();

        void Activate(EngineWindow window);

        void Deactivate(EngineWindow window);

    }
}
