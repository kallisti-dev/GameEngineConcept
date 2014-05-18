using GameEngineConcept.Graphics;
using GameEngineConcept.Graphics.Modes;
using GameEngineConcept.Graphics.VertexBuffers;
using GameEngineConcept.Scenes;
using GameEngineConcept.Util;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace GameEngineConcept
{
    public class EngineWindow : GameWindow
    {
        private static EngineWindow mainWindow = null;

        //since we can't release openGL objects in garbage collector threads, we need to send them to the main thread to be released 
        public static void ReleaseOnMainThread(IRelease releaseObj)
        {
            mainWindow.releaseQueue.SendAsync(releaseObj);
        }


        protected IGraphicsMode currentGraphicsMode;
        protected GameState rootState;
        protected ResourcePool pool;
        BufferBlock<IRelease> releaseQueue = new BufferBlock<IRelease>();
        Dictionary<IScene, PendingScene> loadedScenes = new Dictionary<IScene, PendingScene>();
        Dictionary<IScene, GameState> activeScenes = new Dictionary<IScene, GameState>();

        //internal state related to a background-loaded scene
        private class PendingScene
        {
            public Task task;
            public ResourcePool pool;
            public CancellationTokenSource cancelSource = new CancellationTokenSource();

            public PendingScene(IScene s, ResourcePool p)
            {
                pool = p.Copy();
                task = s.Load(pool, cancelSource.Token);
            }
        }

        public EngineWindow()
            : base(800, 600, GraphicsMode.Default, "foo", GameWindowFlags.Default, null, 3, 0, GraphicsContextFlags.Default)
        {
            rootState = new GameState(this);
            currentGraphicsMode = null;
        }

        public Task LoadScenes(IEnumerable<IScene> scenes)
        {
            return Task.WhenAll(scenes.Select((scene) =>
                Task.Run(() => _LoadScene(scene).Wait())
            ));
        }

        public void UnloadScenes(IEnumerable<IScene> scenes)
        {
            RemoveScenes(scenes);
            foreach (var scene in scenes) {
                PendingScene pending;
                if(loadedScenes.TryGetValue(scene, out pending)) {
                    pending.cancelSource.Cancel(true);
                    pending.pool.Unload();
                    loadedScenes.Remove(scene);
                }
            }
        }

        private Task _LoadScene(IScene scene)
        {
            PendingScene pendingScene;
            if (loadedScenes.TryGetValue(scene, out pendingScene)) {
                return pendingScene.task;
            }
            else {
                return (loadedScenes[scene] = new PendingScene(scene, pool)).task;
            }
        }

        public Task AddScenes(IEnumerable<IScene> scenes)
        {
            return Task.WhenAll(scenes.Select(async (scene) => {
                await _LoadScene(scene);
                var sceneState = new GameState(this, rootState);
                scene.Activate(sceneState);
                activeScenes.Add(scene, sceneState);
            }));
        }

        public Task AddScenes(params IScene[] scenes) { return AddScenes(scenes);  }

        public void RemoveScenes(IEnumerable<IScene> scenes)
        {
            foreach (var scene in activeScenes.Keys) {
                GameState sceneState;
                if (activeScenes.TryGetValue(scene, out sceneState)) {
                    sceneState.Parent = null;
                    activeScenes.Remove(scene);
                }
            }
        }

        public void RemoveScenes(params IScene[] scenes) { RemoveScenes(scenes); }

        /* handling graphics modes */
        public void UseGraphicsMode(IGraphicsMode mode)
        {
            if (currentGraphicsMode != null) {
                currentGraphicsMode.Uninitialize();
            }
            currentGraphicsMode = mode;
            if (currentGraphicsMode != null) {
                currentGraphicsMode.Initialize();
            }
        }

        public void WithGraphicsMode(IGraphicsMode mode, Action inner)
        {
            if (mode == currentGraphicsMode) {
                inner();
                return;
            }
            IGraphicsMode prevGraphicsMode = currentGraphicsMode;
            currentGraphicsMode = mode;
            if (prevGraphicsMode != null) {
                prevGraphicsMode.Uninitialize();
            }
            try {
                mode.Initialize();
                inner();
            }
            finally {
                mode.Uninitialize();
                currentGraphicsMode = prevGraphicsMode;
                if (currentGraphicsMode != null)
                    currentGraphicsMode.Initialize();
            }
        }

        /* event handlers */
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UseGraphicsMode(new Texturing2DMode());
            pool = new ResourcePool(
                new Pool<Texture>(Texture.Allocate),
                new Pool<VertexBuffer>(VertexBuffer.Allocate)
            );
            Debug.Assert(mainWindow == null);
            mainWindow = this;
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            rootState.Drawables.Draw();
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            rootState.Components.Update();
            processReleaseQueue();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            //WithGraphicsMode(new ResizeMode(Width, Height), () => { });
        }

        private void processReleaseQueue()
        {
            IList<IRelease> releaseList;
            if (releaseQueue.TryReceiveAll(out releaseList)) {
                foreach (var r in releaseList) { r.Release(); }
            }
        }
    }
}
