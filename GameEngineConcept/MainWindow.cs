using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;
using OpenTK;
using OpenTK.Platform;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using GameEngineConcept.Graphics;
using GameEngineConcept.Graphics.Modes;
using GameEngineConcept.Components;

namespace GameEngineConcept
{
    public class MainWindow : GameWindow
    {
        private static MainWindow mainWindow = null;

        HashSet<IDrawable> drawSet = new HashSet<IDrawable>();
        HashSet<IComponent> updateSet = new HashSet<IComponent>();
        
        IGraphicsMode graphicsMode = null;

        public MainWindow() : base(800, 600, GraphicsMode.Default, "foo", GameWindowFlags.Default, null, 4, 2, GraphicsContextFlags.Debug) {  }

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
            //UseGraphicsMode(new ResizeMode(Width, Height));
            //UseGraphicsMode(new Texturing2DMode());
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
            foreach (IComponent c in updateSet) { c.Update(); }
            base.OnUpdateFrame(e);
        }

        protected override void OnResize(EventArgs e)
        {
            WithGraphicsMode(new ResizeMode(Width, Height), () => { });
            base.OnResize(e);
        }
    }
}
