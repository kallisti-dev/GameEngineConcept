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
    public class JoinScenes : Scene
    {
        IScene[] scenes;

        public override bool IsLoaded { get { return Array.TrueForAll(scenes, (s) => s.IsLoaded);  } }

        public JoinScenes(params IScene[] scenes)
        {
            this.scenes = scenes;
        }

        public override void Load()
        {
            foreach(var scene in scenes) { scene.Load(); }
        }

        public override void Unload()
        {
            foreach(var scene in scenes.Reverse<IScene>()) { scene.Unload(); }
        }

        public override void Activate(EngineWindow w)
        {
            foreach (var scene in scenes) { scene.Activate(w); }
        }

        public override void Deactivate(EngineWindow w)
        {
            foreach (var scene in scenes.Reverse<IScene>()) { scene.Deactivate(w); }
        }
    }
}
