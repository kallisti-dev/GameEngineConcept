using GameEngineConcept;
using GameEngineConcept.Util;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GameEngineTest
{
    public class TestWindow : EngineWindow
    {
        Queue<BaseTester> tests = new Queue<BaseTester>();
        BaseTester currentTest = null;
        int testsSucceeded = 0;
        int testsFailed = 0;

        public TestWindow() : base() { }

        public void AddTest(BaseTester test) 
        { tests.Enqueue(test); }
        
        public void AddTest<T>() where T : BaseTester, new() 
        { AddTest(new T()); }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            StartNextTest();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            TestFailOnException(() => currentTest.OnRenderFrame(e));
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            TestFailOnException(() => currentTest.OnUpdateFrame(e));
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            TestFailOnException(() => currentTest.OnResize());
        }

        private void StartNextTest()
        {
            if (currentTest != null) {
                currentTest.OnUnload(rootState);
                currentTest.TestComplete -= TestCompleteHandler;
            }
            currentTest = tests.Dequeue();
            currentTest.TestComplete += TestCompleteHandler;
            TestFailOnException(() => currentTest.OnLoad(rootState));
        }

        //event handler for test completion
        //keeps success/fail count and any other useful data
        private void TestCompleteHandler(BaseTester sender, bool success)
        {
            Debug.Assert(sender == currentTest);

            if (success) {
                testsSucceeded++;
                Debug.Print(sender.GetType() + " succeeded.");
            }
            else {
                testsFailed++;
                Debug.Print(sender.GetType() + " failed.");
            }

            if (tests.Count > 0)
                StartNextTest();
            else
                Console.WriteLine("All tests completed. Total: " + (testsSucceeded + testsFailed) + " Succeess: " + testsSucceeded + " Failed: " + testsFailed);
        }

        //Fails the current test if an exception in the given Action occurs
        private void TestFailOnException(Action testAction)
        {
            try { testAction(); }
            catch (Exception ex) {
                Debug.Print(ex.StackTrace.ToString());
                Debugger.Break();
                currentTest.TestFailure();
                return;
            }
            if (ErrorCode.NoError != Util.TraceGLError()) {
                currentTest.TestFailure();
            }
            
        }
    }
}
