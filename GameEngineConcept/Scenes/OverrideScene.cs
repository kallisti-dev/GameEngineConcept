using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenTK;

using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineConcept.Scenes
{
    //base class for scenes that removes all current scenes when activated, and restores them once deactivated
    public abstract class OverrideScene : Scene
    {
        IEnumerable<IScene> scenes = null;

        public abstract override bool IsLoaded { get; }

        public override abstract Task Load(Pool<VertexBuffer> pool);

        public override abstract void Unload(Pool<VertexBuffer> pool);

        public override void Activate(EngineWindow w)
        {
            scenes = w.RemoveAllScenes();
        }

        public override void Deactivate(EngineWindow w)
        {
            w.AddScenes(scenes);
        }
    }
}
