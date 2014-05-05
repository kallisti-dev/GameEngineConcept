using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameEngineConcept;

namespace GameEngineConcept.Scenes
{
    //Combine multiples scenes into one.
    //
    //Scenes are loaded/activated in order given,
    //and unloaded/deactivated in reverse order.
    public class JoinScenes : IScene
    {
        IScene[] scenes;

        public bool IsLoaded { get { return Array.TrueForAll(scenes, (s) => s.IsLoaded);  } }

        public JoinScenes(params IScene[] scenes)
        {
            this.scenes = scenes;
        }

        public void Load()
        {
            foreach(var scene in scenes) { scene.Load(); }
        }

        public void Unload()
        {
            foreach(var scene in scenes.Reverse()) { scene.Unload(); }
        }

        public void Activate(EngineWindow w)
        {
            foreach (var scene in scenes) { scene.Activate(w); }
        }

        public void Deactivate(EngineWindow w)
        {
            foreach (var scene in scenes.Reverse()) { scene.Deactivate(w); }
        }
    }
}
