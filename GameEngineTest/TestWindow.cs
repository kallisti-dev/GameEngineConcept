﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Platform;

using GameEngineConcept;

namespace GameEngineTest
{
    class TestWindow : MainWindow
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

        private void TestCompleteHandler(BaseTester sender, bool success) 
        {
            Debug.Assert(sender == currentTest);

            if (success)
            {
                testsSucceeded++;
                Console.WriteLine(sender.GetType() + " succeeded.");
            }
            else
            {
                testsFailed++;
                Console.WriteLine(sender.GetType() + " failed.");
            }

            if (tests.Count > 0)
                StartNextTest();
            else
                Console.WriteLine("All tests completed. Total: " + (testsSucceeded + testsFailed) + " Succeess: " + testsSucceeded + " Failed: " + testsFailed);
        }

        private void StartNextTest()
        {
            if (currentTest != null)
            {
                currentTest.OnUnload();
                currentTest.TestComplete -= TestCompleteHandler;
            }
            currentTest = tests.Dequeue();
            currentTest.TestComplete += TestCompleteHandler;
            currentTest.OnLoad();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            StartNextTest();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
           base.OnRenderFrame(e);
           currentTest.OnRenderFrame(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            currentTest.OnUpdateFrame(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            currentTest.OnResize();
        }
    }
}
