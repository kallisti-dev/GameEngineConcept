using System;
using GameEngineConcept.Graphics.VertexBuffers;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GameEngineConcept.Scenes
{

    //abstract base class for Scenes
    //static convenience functions for Scenes go here as well.
    public abstract class Scene
    {
        public static Scene Join(params Scene[] scenes) { return new JoinScenes(scenes);  }

        public static Scene operator+(Scene s1, Scene s2) { return new JoinScenes(s1, s2); }


        public abstract Task Load(ResourcePool pool, CancellationToken token);

        public abstract void Activate(GameState window);
    }
}
