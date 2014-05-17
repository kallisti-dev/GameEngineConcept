using System;
using System.Windows.Forms;
using System.Diagnostics;
using OpenTK;

namespace GameEngineTest
{
    public abstract class BaseTester
    {

        public event Action<BaseTester, bool> TestComplete;

        public virtual bool SucceedOnTimeout { get; protected set; }
        public virtual int FramesUntilTimeout { get; protected set; }

        protected BaseTester()
        {
            SucceedOnTimeout = false;
            FramesUntilTimeout = 300;
        }

        public void TestSuccess()
        {
            TestComplete(this, true);
        }

        public void TestFailure()
        {
            TestComplete(this, false);
        }

        public virtual void OnLoad(TestWindow window) { }
        public virtual void OnUnload(TestWindow window) { }
        public virtual void OnRenderFrame(FrameEventArgs e) { }
        public virtual void OnUpdateFrame(FrameEventArgs e) 
        {
            CheckTimeout();
        }
        public virtual void OnResize() { }

        private void CheckTimeout()
        {
            Debug.Print("Timeout is called " + FramesUntilTimeout);
            if (FramesUntilTimeout <= 0)
            {
                if (SucceedOnTimeout)
                {
                    TestSuccess();
                }
                else TestFailure();
            }
            else FramesUntilTimeout--;
        }
    }
}
