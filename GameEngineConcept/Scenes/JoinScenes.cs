using System;
using System.Linq;
using System.Threading.Tasks;

using GameEngineConcept.Graphics.VertexBuffers;

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

        public override Task Load(Pool<VertexBuffer> pool)
        {
            return Task.WhenAll(scenes.Select((scene) => scene.Load(pool)));      
        }

        public override void Unload(Pool<VertexBuffer> pool)
        {
            foreach(var scene in scenes.Reverse<IScene>()) { scene.Unload(pool); }
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
