using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineTest.Tests
{
    //abstract base class for draw tests
    public abstract class DrawTester : BaseTester
    {
        protected DrawTester()
            : base()
        {
            SucceedOnTimeout = true;
        }
    }
}
