using GameEngineConcept;
using OpenTK;
using System;
using System.Diagnostics;

namespace GameEngineTest
{
    public abstract class BaseTester
    {

        public event Action<BaseTester, bool> TestComplete;

        public bool SucceedOnTimeout { get; protected set; }
        public int FramesUntilTimeout { get; protected set; }

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

        public virtual void OnLoad(GameState window) { }
        public virtual void OnUnload(GameState window) { }
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
