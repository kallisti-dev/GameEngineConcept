using GameEngineConcept.Graphics.VertexBuffers;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GameEngineConcept.Scenes
{
    //Combine multiples scenes into one.
    //
    //Scenes are loaded/activated in order given,
    //and unloaded/deactivated in reverse order.
    public class JoinScenes : Scene
    {
        Scene[] scenes;

        public JoinScenes(params Scene[] scenes)
        {
            this.scenes = scenes;
        }

        public override Task Load(ResourcePool pool, CancellationToken token)
        {
            return Task.WhenAll(scenes.Select((scene) => scene.Load(pool, token)));      
        }

        public override void Activate(GameState s)
        {
            foreach (var scene in scenes) { scene.Activate(s); }
        }
    }
}
