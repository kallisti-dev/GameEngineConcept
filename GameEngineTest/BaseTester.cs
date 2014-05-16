﻿using System;
using System.Windows.Forms;
using OpenTK;

namespace GameEngineTest
{
    public abstract class BaseTester
    {

        public event Action<BaseTester, bool> TestComplete;
        private int timeoutCounter = 300; //Assuming 60 cycles a second then this is 5 seconds
        private bool succeedOnTimeout = false;

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
            Console.Out.WriteLine("Timeout is called" + timeoutCounter);
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
        public bool SucceedOnTimeout
        {
            get { return succeedOnTimeout; }
            set { succeedOnTimeout = value; }
        }
    }
}
