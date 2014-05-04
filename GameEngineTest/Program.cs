using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using GameEngineConcept;
using GameEngineTest.Tests;

namespace GameEngineTest
{
    /* simple test system */
    class Program
    {
        static void Main(string[] args)
        {
            using (TestWindow window = new TestWindow())
            {
                window.AddTest<TestWindowTest>();
                window.Run();
            }
        }
    }
}
