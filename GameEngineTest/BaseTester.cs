using System;
using OpenTK;

namespace GameEngineTest
{
    public abstract class BaseTester
    {

        public event Action<bool> TestComplete;

        public void TestSuccess()
        {
            TestComplete(true);
        }

        public void TestFailure()
        {
            TestComplete(false);
        }

        public virtual void OnLoad() { }
        public virtual void OnUnload() { }
        public virtual void OnRenderFrame(FrameEventArgs e) { }
        public virtual void OnUpdateFrame(FrameEventArgs e) { }
        public virtual void OnResize() { }
    }
}
