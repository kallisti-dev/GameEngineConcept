using GameEngineConcept.Graphics;
using GameEngineConcept.Graphics.Modes;
using GameEngineConcept.Graphics.VertexBuffers;
using GameEngineConcept.Util;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
 

        public EngineWindow()
            : base(800, 600, GraphicsMode.Default, "foo", GameWindowFlags.Default, null, 3, 0, GraphicsContextFlags.Default)
        {
            currentGraphicsMode = null;
        }

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
            rootState = new GameState(this, pool);
            Debug.Assert(mainWindow == null);
            mainWindow = this;
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            rootState.OnShutdown(this);
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
            rootState.UpdateComponents();
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
