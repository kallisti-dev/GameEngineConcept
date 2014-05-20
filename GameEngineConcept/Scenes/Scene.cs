using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GameEngineConcept.Scenes
{

    //abstract base class for Scenes
    //static convenience functions for Scenes go here as well.
    public abstract class Scene
    {
        public static Scene Join(params Scene[] scenes) { return new JoinScenes(scenes);  }
        public static Scene operator+(Scene s1, Scene s2) { return new JoinScenes(s1, s2); }

        static readonly Dictionary<Scene, PendingScene> loadedScenes = new Dictionary<Scene, PendingScene>();

        public static Task Load(Scene scene, ResourcePool pool)
        {
            return Task.Run(() => _Load(scene, pool).Wait());
        }

        public static void Unload(Scene scene)
        {
            PendingScene pending;
            lock (loadedScenes) {
                if (loadedScenes.TryGetValue(scene, out pending)) {
                    pending.cancelSource.Cancel(true);
                    pending.pool.Release();
                    loadedScenes.Remove(scene);
                    scene.OnUnload(scene);
                }
            }
        }

        private static Task _Load(Scene scene, ResourcePool pool)
        {
            PendingScene pendingScene;
            lock (loadedScenes) {
                if (loadedScenes.TryGetValue(scene, out pendingScene)) {
                    return pendingScene.task;
                }
                else {
                    return (loadedScenes[scene] = new PendingScene(scene, pool)).task;
                }
            }
        }

        //internal state related to a background-loaded scene
        private class PendingScene
        {
            public Task task;
            public ResourcePool pool;
            public CancellationTokenSource cancelSource = new CancellationTokenSource();

            public PendingScene(Scene s, ResourcePool p)
            {
                pool = p.Copy();
                task = s.Load(pool, cancelSource.Token);
            }
        }


        public event Action<Scene> OnUnload;

        public abstract Task Load(ResourcePool pool, CancellationToken token);

        public abstract void Activate(GameState state);
    }
}
