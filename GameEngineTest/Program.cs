using GameEngineTest.Tests;
using System;

namespace GameEngineTest
{
    /* simple test system */
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (TestWindow window = new TestWindow())
            {
                window.AddTest<TestWindowTest>();
                window.AddTest<BroadcastTestComponent>();
                window.AddTest<Triangle2DDrawTest>();
                window.AddTest<SpriteDrawTest>();
                window.Run();
            }
        }
    }
}
