using System;
using System.Collections.Generic;
using OpenTK;

namespace GameEngineConcept.Scenes
{
    //base class for scenes that override all current scenes when activated, and restore them once deactivated
    public abstract class OverrideScene : Scene
    {
        IEnumerable<IScene> scenes = null;

        public abstract override bool IsLoaded { get; }

        public override abstract void Load();

        public override abstract void Unload();

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
