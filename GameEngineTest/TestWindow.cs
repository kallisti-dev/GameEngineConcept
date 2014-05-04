using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameEngineConcept;

namespace GameEngineTest
{
    class TestWindow : MainWindow
    {
        List<ITester> tests;

        public void AddTest(ITester test) 
        { tests.Add(test); }
        public void AddTest<T>() where T : ITester, new() 
        { AddTest(new T()); }
    }
}
