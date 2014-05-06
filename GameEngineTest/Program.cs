using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Threading;

using OpenTK;

using GameEngineConcept;
using GameEngineTest.Tests;

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
                window.Run(60, 60);
            }
        }
    }
}
