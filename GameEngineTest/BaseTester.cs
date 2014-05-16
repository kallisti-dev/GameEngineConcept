using System;
using System.Windows.Forms;
using System.Diagnostics;
using OpenTK;

namespace GameEngineTest
{
    public abstract class BaseTester
    {

        public event Action<BaseTester, bool> TestComplete;
        private int timeoutCounter = 300; //Assuming 60 cycles a second then this is 5 seconds

        public virtual bool SucceedOnTimeout { get; protected set; }

        protected BaseTester()
        {
            SucceedOnTimeout = false;
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
            Timeout();
        }
        public virtual void OnResize() { }
        public virtual void Timeout()
        {
            Debug.Print("Timeout is called " + timeoutCounter);
            if (timeoutCounter <= 0)
            {
                if (SucceedOnTimeout)
                {
                    TestSuccess();
                }
                else TestFailure();
            }
            else timeoutCounter--;
        }
    }
}
