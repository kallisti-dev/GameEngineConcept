using System;
using System.Windows.Forms;
using OpenTK;

namespace GameEngineTest
{
    public abstract class BaseTester
    {

        public event Action<BaseTester, bool> TestComplete;

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
        public virtual void OnUpdateFrame(FrameEventArgs e) { }
        public virtual void OnResize() { }
    }
}
