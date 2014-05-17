using GameEngineConcept.Graphics;
using GameEngineConcept.Graphics.Modes;
using GameEngineConcept.Graphics.VertexBuffers;
using GameEngineConcept.Scenes;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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


        Dictionary<IScene, GameState> sceneSet = new Dictionary<IScene, GameState>();
        BufferBlock<IRelease> releaseQueue = new BufferBlock<IRelease>();
        Pool<VertexBuffer> vPool;
        public GameState RootState { get; private set; }   //root game state for this window

        public IGraphicsMode CurrentGraphicsMode { get; private set; }

        public EngineWindow()
            : base(800, 600, GraphicsMode.Default, "foo", GameWindowFlags.Default, null, 3, 0, GraphicsContextFlags.Default)
        {
            RootState = new GameState(this);
            CurrentGraphicsMode = null;
        }

        public Task AddScenes(IEnumerable<IScene> scenes)
        {
            return Task.WhenAll(scenes.Select(async (scene) => {
                if (!scene.IsLoaded)
                    await scene.Load(vPool);
                var sceneState = new GameState(this, RootState);
                scene.Activate(sceneState);
                sceneSet.Add(scene, sceneState);
            }));
        }

        public Task AddScenes(params IScene[] scenes) { return AddScenes(scenes);  }

        public void RemoveScene(IScene scene)
        {
        }

        public void RemoveScenes(IEnumerable<IScene> scenes)
        {
            foreach (var scene in sceneSet.Keys) {
                GameState sceneState;
                if (sceneSet.TryGetValue(scene, out sceneState)) {
                    sceneState.Parent = null;
                    sceneSet.Remove(scene);
                }
            }
        }

        public void RemoveScenes(params IScene[] scenes) { RemoveScenes(scenes); }

        //removes all scenes from the window. returns a collection of the scenes that can later be
        //passed to AddScenes to restore them.
        public IEnumerable<IScene> RemoveAllScenes()
        {
            var scenesCopy = new HashSet<IScene>(sceneSet.Keys);
            RemoveScenes(sceneSet.Keys);
            return scenesCopy;
        }

        public void UseGraphicsMode(IGraphicsMode mode)
        {
            if (CurrentGraphicsMode != null) {
                CurrentGraphicsMode.Uninitialize();
            }
            CurrentGraphicsMode = mode;
            if (CurrentGraphicsMode != null) {
                CurrentGraphicsMode.Initialize();
            }
        }

        public void WithGraphicsMode(IGraphicsMode mode, Action inner)
        {
            if (mode == CurrentGraphicsMode) {
                inner();
                return;
            }
            IGraphicsMode prevGraphicsMode = CurrentGraphicsMode;
            CurrentGraphicsMode = mode;
            if (prevGraphicsMode != null) {
                prevGraphicsMode.Uninitialize();
            }
            try {
                mode.Initialize();
                inner();
            }
            finally {
                mode.Uninitialize();
                CurrentGraphicsMode = prevGraphicsMode;
                if (CurrentGraphicsMode != null)
                    CurrentGraphicsMode.Initialize();
            }
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UseGraphicsMode(new Texturing2DMode());
            vPool = Pool.CreateBufferPool();
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
            //SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            processReleaseQueue();
            base.OnUpdateFrame(e);
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
