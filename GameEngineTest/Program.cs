using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace GameEngineTest
{
    /* simple test system */
    class Program
    {
        static int failCount = 0;


        static void RunTest(Type t)
        {
            MethodInfo m = t.GetMethod("Test");
            if (m == null || !m.IsStatic || m.GetParameters().Length > 0 || !typeof(bool).IsAssignableFrom(m.ReturnType))
            {
                Console.WriteLine("warning: no method with signature \"static bool Test()\" for type " + t);
            }
            else if ((bool)m.Invoke(null, null))
            {
                Console.WriteLine(t + " succeeded.");
            }
            else
            {
                Console.WriteLine(t + " failed.");
                ++failCount;
            }
        }

        static void RunTest<T>()
        {
            RunTest(typeof(T));
        }

        static void Main(string[] args)
        {
            RunTest<BroadcastTestComponent>();
            Console.WriteLine(failCount + " test(s) failed.");
        }
    }
}
