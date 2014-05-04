using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Diagnostics;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using GameEngineConcept.Graphics;

namespace GameEngineConcept
{
    public class MainWindow : GameWindow
    {

        private static BufferBlock<Action> drawQueue = new BufferBlock<Action>();
        private static MainWindow mainWindow = null;

        public static Task<bool> WithDrawThread(Action callback) 
        {
            return drawQueue.SendAsync(callback);
        }

        public MainWindow() : base(500, 500, GraphicsMode.Default, "test") {  }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Debug.Assert(mainWindow == null);
            mainWindow = this;
            GL.ClearColor(Color.Black);
            Texture.Initialize2DTexturing();
        }

        protected override void OnUnload(EventArgs e)
        {
            drawQueue.Complete();
            base.OnUnload(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            processDrawQueue();
            SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }

        protected override void OnResize(EventArgs e)
        {
            // Standard OpenTK code for window resize
            GL.Viewport(0, 0, Width, Height);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 1.0f, 64.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
            base.OnResize(e);
        }

        private void processDrawQueue() 
        {
            IList<Action> drawActions;
            if (drawQueue.TryReceiveAll(out drawActions))
            {
                foreach (var drawAction in drawActions)
                    drawAction();
            }
        }
    }
}
