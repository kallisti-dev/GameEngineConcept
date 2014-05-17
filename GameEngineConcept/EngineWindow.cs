using GameEngineConcept.Components;
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

        HashSet<IScene> sceneSet = new HashSet<IScene>();
        ComponentCollection updateSet = new ComponentCollection();
        HashSet<IDrawable> drawSet;
        DrawableDepthSet depthSet = new DrawableDepthSet();
        BufferBlock<IRelease> releaseQueue = new BufferBlock<IRelease>();
        Pool<VertexBuffer> vPool;

        public IGraphicsMode CurrentGraphicsMode { get; private set; }

        public EngineWindow()
            : base(800, 600, GraphicsMode.Default, "foo", GameWindowFlags.Default, null, 3, 0, GraphicsContextFlags.Default)
        {
            drawSet = new HashSet<IDrawable> { depthSet };
            CurrentGraphicsMode = null;
        }

        public void AddDrawables(IEnumerable<IDrawableDepth> drawables)
        {
            depthSet.UnionWith(drawables);
        }

        public void AddDrawables(IEnumerable<IDrawable> drawables)
        {
            drawSet.UnionWith(drawables);
        }

        public void RemoveDrawables(IEnumerable<IDrawableDepth> drawables)
        {
            depthSet.ExceptWith(drawables);
        }

        public void RemoveDrawables(IEnumerable<IDrawable> drawables)
        {
            drawSet.ExceptWith(drawables);
        }

        public void AddComponent(IComponent component)
        {
            updateSet.Add(component);
        }

        public void AddComponents(IEnumerable<IComponent> components)
        {
            updateSet.UnionWith(components);
        }

        public void RemoveComponents(IEnumerable<IComponent> components)
        {
            updateSet.ExceptWith(components);
        }

        public void RemoveComponent(IComponent components)
        {
            updateSet.Remove(components);
        }

        public async Task AddScene(IScene scene)
        {
            if (!scene.IsLoaded)
                await scene.Load(vPool);
            scene.Activate(this);
            sceneSet.Add(scene);
        }

        public Task AddScenes(IEnumerable<IScene> scenes)
        {
            return Task.WhenAll(scenes.Select(AddScene));
        }

        public void RemoveScene(IScene scene)
        {
            if (sceneSet.Remove(scene)) {
                scene.Deactivate(this);
            }
        }

        public void RemoveScenes(IEnumerable<IScene> scenes)
        {
            foreach (var s in sceneSet) { RemoveScene(s); }
        }

        //removes all scenes from the window. returns a collection of the scenes that can later be
        //passed to AddScenes to restore them.
        public IEnumerable<IScene> RemoveAllScenes()
        {
            var scenesCopy = new HashSet<IScene>(sceneSet);
            RemoveScenes(sceneSet);
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
            foreach (IDrawable x in drawSet) { x.Draw(); }
            base.OnRenderFrame(e);
            //SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            processReleaseQueue();
            updateSet.Update<AnimationComponent>();
            updateSet.Update<MiscellaneousComponent>();
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
