using System;
using OpenTK;

namespace GameEngineTest.Tests
{
    //tests that the test system is working properly
    class TestWindowTest : BaseTester
    {

        static int maxCount = 10;
        int count = 0;

        public  override void OnLoad() { count++; }
        public  override void OnRenderFrame(FrameEventArgs e) { count++; }
        public  override void OnUpdateFrame(FrameEventArgs e) 
        { 
            count++;
            if (count >= maxCount)
                TestSuccess();
        }

    }
}
