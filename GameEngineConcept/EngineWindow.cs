using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Diagnostics;
using System.Drawing;
using OpenTK;
using OpenTK.Platform;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using GameEngineConcept.Scenes;
using GameEngineConcept.Graphics;
using GameEngineConcept.Graphics.Modes;
using GameEngineConcept.Components;

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
        HashSet<IComponent> updateSet = new HashSet<IComponent>();
        HashSet<IDrawable> drawSet;
        DrawableDepthSet depthSet = new DrawableDepthSet();
        BufferBlock<IRelease> releaseQueue = new BufferBlock<IRelease>();     
        IGraphicsMode graphicsMode = null;

        public EngineWindow() : base(800, 600, GraphicsMode.Default, "foo", GameWindowFlags.Default, null, 4, 2, GraphicsContextFlags.Debug) 
        {
            drawSet = new HashSet<IDrawable> { depthSet };
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

        public void AddComponents(IEnumerable<IComponent> components)
        {
            updateSet.UnionWith(components);
        }

        public void RemoveComponents(IEnumerable<IComponent> components)
        {
            updateSet.ExceptWith(components);
        }

        public void AddScene(IScene scene)
        {
            if(scene.IsLoaded)
                scene.Load();
            scene.Activate(this);
            sceneSet.Add(scene);
        }

        public async void AddSceneAsync(Task<IScene> sceneTask)
        {
            AddScene(await sceneTask);
        }

        public void RemoveScene(IScene scene)
        {
            if (sceneSet.Remove(scene))
            {
                scene.Deactivate(this);
            }
        }

        public void UseGraphicsMode(IGraphicsMode mode)
        {
            if (graphicsMode != null)
                graphicsMode.Uninitialize();
            graphicsMode = mode;
            if (graphicsMode != null)
                graphicsMode.Initialize();

        }

        public void WithGraphicsMode(IGraphicsMode mode, Action inner)
        {
            if (mode == graphicsMode) 
            {
                inner();
                return;
            }
            IGraphicsMode prevGraphicsMode = graphicsMode;
            graphicsMode = mode;
            MatrixMode? mMode = mode.PrimaryMatrixMode, prevMMode = null;
            if (prevGraphicsMode != null)
            {
                prevMMode = prevGraphicsMode.PrimaryMatrixMode;
                prevGraphicsMode.Uninitialize();
            }   
            bool restore;
            if (restore = prevMMode.HasValue && mMode == prevMMode)
            {
                GL.PushMatrix();
            }
            else if (mMode.HasValue)
            {
                GL.MatrixMode(mMode.Value);
            }
            try
            {
                mode.Initialize();
                inner();
            }
            finally
            {
                mode.Uninitialize();
                if (restore)
                {
                    GL.PopMatrix();
                }
                else if (prevMMode.HasValue)
                {
                    GL.MatrixMode(prevMMode.Value);
                }
                graphicsMode = prevGraphicsMode;
                if(graphicsMode != null)
                    graphicsMode.Initialize();
            }
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Debug.Assert(mainWindow == null);
            mainWindow = this;
            UseGraphicsMode(new ResizeMode(Width, Height));
            UseGraphicsMode(new Texturing2DMode());
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
            foreach (IComponent c in updateSet) { c.Update(); }
            base.OnUpdateFrame(e);
        }

        protected override void OnResize(EventArgs e)
        {
            WithGraphicsMode(new ResizeMode(Width, Height), () => { });
            base.OnResize(e);
        }

        private void processReleaseQueue()
        {
            IList<IRelease> releaseList;
            if(releaseQueue.TryReceiveAll(out releaseList))
            {
                foreach (var r in releaseList) { r.Release(); }
            }
        }
    }
}
