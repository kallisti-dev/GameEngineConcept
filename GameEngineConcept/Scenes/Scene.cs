using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineConcept.Scenes
{
    //abstract base class for Scenes
    //static convenience functions for Scenes go here as well.
    public abstract class Scene : IScene
    {

        public static IScene Join(params IScene[] scenes) { return new JoinScenes(scenes);  }

        public static IScene operator+(Scene s1, IScene s2) { return new JoinScenes(s1, s2); }
        public static IScene operator+(IScene s1, Scene s2) { return new JoinScenes(s1, s2); }

        public static Task LoadInBackground(IScene scene)
        {
            return Task.Run(() => scene.Load());
        }

        public abstract bool IsLoaded { get; }

        public abstract void Load();

        public abstract void Unload();

        public abstract void Activate(EngineWindow window);

        public abstract void Deactivate(EngineWindow window);
    }
}
