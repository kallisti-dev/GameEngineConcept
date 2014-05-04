using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Diagnostics;
using OpenTK;

namespace GameEngineConcept
{
    class MainWindow : GameWindow
    {

        private static BufferBlock<Action> drawQueue = new BufferBlock<Action>();
        private static MainWindow mainWindow = null;

        public static Task<bool> WithDrawThread(Action callback) 
        {
            return drawQueue.SendAsync(callback);
        }

        public MainWindow() : base() {  }

        protected override void OnLoad(EventArgs e)
        {
            Debug.Assert(mainWindow == null);
            mainWindow = this;    
        }

        protected override void OnUnload(EventArgs e)
        {
            drawQueue.Complete();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            processDrawQueue();
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
